import React from 'react';

import {
  ComponentParams,
  Field,
  ImageField,
  Text,
  ComponentRendering,
  DateField,
  GetStaticComponentProps,
} from '@sitecore-jss/sitecore-jss-nextjs';

import graphqlClientFactory from 'lib/graphql-client-factory';
import { Image } from 'src/atoms/BlogComponents/JssImage';

interface BlogAuthor {
  targetItem: BlogAuthorFields;
}

interface BlogAuthorFields {
  name: Field<string>;
  profileImage: ImageField;
  bio: Field<string>;
}

interface BlogCategory {
  targetItems: BlogCategoryFields[];
}

interface BlogCategoryFields {
  name: Field<string>;
}

interface BlogTag {
  targetItems: BlogTagFields[];
}

interface BlogTagFields {
  name: Field<string>;
}

interface BlogPost {
  id: string
  title: Field<string>;
  summary: Field<string>;
  content: Field<string>;
  featuredImage: ImageField & { metadata?: { [key: string]: unknown } };
  publishDate: Field<string>;
  author: BlogAuthor;
  categories: BlogCategory[];
  tags: BlogTag[];
}

interface Fields {
  BlogListingTitle: Field<string>;
  BlogListingDescription: Field<string>;
  items: BlogPost[];
  datasource: {
    blogFolderPath: Field<string>;
    pageSize: Field<number>;
  };
}

interface BlogListingProps {
  rendering: ComponentRendering & { params: ComponentParams };
  params: ComponentParams;
  fields: Fields;
  blogPosts: BlogPost[];
}

const GET_BLOG_POSTS = `query GetBlogPosts($dataSource: String!, $first: Int!, $after: String) {
  search(
    where: {
      AND: [
        { name: "_path", value: $dataSource, operator: CONTAINS }
        { name: "_templates", value: "{3F65F364-D34D-4D7D-AF0C-40AB9571D2F9}", operator: EQ }
      ]
    }, first: $first, after: $after
    orderBy: { name: "blogPublishDate", direction: DESC }
  ) {
    total
    pageInfo {
      endCursor
      hasNext
    }
    results {
      id
      ... on BlogPost {
        title: blogHeading {
          value
        }
        summary: blogSummary {
          value
        }
        featuredImage: blogImage {
          value
          src
          alt
        }
        publishDate: blogPublishDate {
          value
        }
        author: blogAuthor {
          targetItem {
            ... on BlogAuthor {
              name: blogAuthorName {
                value
              }
              bio: blogAuthorBio {
                value
              }
              profileImage: blogAuthorPhoto {
                value
                src
                alt
              }
            }
          }
        }
        categories: blogCategories {
          targetItems {
            ... on BlogCategory {
              name: blogCategoryName {
                value
              }
            }
          }
        }
        tags: blogTags {
          targetItems {
            ... on BlogTag {
              name: blogTagName {
                value
              }
            }
          }
        }
      }
    }
  }
}`;

interface FeaturedImage {
  value: ImageField
  src: string;
  alt: string;
}
export interface ImageProps {
  field: FeaturedImage;
}

export const Default = (props: BlogListingProps): JSX.Element => {
  const id = props.params.RenderingIdentifier;
  const posts = props.blogPosts;

  return (
    <div className={`component ${props.params.styles}`} id={id || undefined}>
      <div className="component-content px-3">
        <h1 className='blog-title text-center'>{props.fields.BlogListingTitle?.value}</h1>
        <p className='blog-description text-center'>{props.fields.BlogListingDescription?.value}</p>

        {posts && posts.map((blog: BlogPost, index) => (
          <>
            <div key={blog.id} className="blog-post">

              <Text tag="h2" field={blog.title} />
              <div className="blog-featuredImage">
                <Image field={blog.featuredImage} />
              </div>
              <Text tag="p" field={blog.summary} />
              <p>
                Publish Date:&nbsp;
                <DateField field={blog.publishDate}
                    render={() => {
                      const isoFormatted = blog.publishDate.value.replace(
                        /^(\d{4})(\d{2})(\d{2})T(\d{2})(\d{2})(\d{2})Z$/,
                        "$1-$2-$3T$4:$5:$6Z"
                      );
                      const dateObj = new Date(isoFormatted);

                      // Extract month, day, and year using UTC methods
                      const month = ("0" + (dateObj.getUTCMonth() + 1)).slice(-2); // Months are 0-indexed
                      const day = ("0" + dateObj.getUTCDate()).slice(-2);
                      const year = dateObj.getUTCFullYear();

                      // Format the date as MM/DD/YYYY
                      const formattedDate = `${month}/${day}/${year}`;

                      return formattedDate;
                    }}/>

              </p>
              <p>
                Author:&nbsp;
                <Text tag="span" field={blog.author.targetItem.name} />
              </p>
              <p>
                Category:&nbsp;
                {blog.categories?.targetItems?.map((category, index) => (
                  <span key={index}>
                    {index > 0 ? ', ' : ''}
                    {category.name.value}
                  </span>
                ))}
              </p>
              <p>
                Tags:&nbsp;
                {blog.tags?.targetItems?.map((tag, index) => (
                  <span key={index}>
                    {index > 0 ? ', ' : ''}
                    {tag.name.value}
                  </span>
                ))}
              </p>
            </div>
          {index < (posts.length - 1) && <hr className='blog-divider my-5'/>}
          </>
        ))}
      </div>
    </div>
  );
}

export const getStaticProps: GetStaticComponentProps = async (rendering, layoutData) => {
  const graphQLClient = graphqlClientFactory();

  const response = await graphQLClient.request<BlogPost[]>(GET_BLOG_POSTS, {
    dataSource: rendering.dataSource ?? '{0DE95AE4-41AB-4D01-9EB0-67441B7C2450}',
    first: 10,
    after: "",
    language: layoutData.sitecore.context.language,
  });

  return {
    blogPosts: response?.search?.results || [],
  };
};