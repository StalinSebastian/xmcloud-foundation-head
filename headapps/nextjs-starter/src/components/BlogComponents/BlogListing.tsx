import React from 'react';
import { useEffect, useState } from 'react';

import {
  ComponentParams,
  Field,
  ImageField,
  Text,
  Image as JssImage,
  ComponentRendering,
  DateField,
} from '@sitecore-jss/sitecore-jss-nextjs';

import { GraphQLRequestClient } from '@sitecore-jss/sitecore-jss-nextjs/graphql';

const graphQLClient = new GraphQLRequestClient('https://xmcloudcm.localhost/sitecore/api/graph/edge', {
  apiKey: 'c0a7c9d5-b821-4656-b1e1-e219e0a2a2f2',
});

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
  author: BlogAuthorFields;
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
}

const GET_BLOG_POSTS = `query GetBlogPosts($dataSource: String!, $first: Int!, $after: String) {
  search(
    where: {
      AND: [
        { name: "_path", value: $dataSource, operator: CONTAINS }
        {
          name: "_templates"
          value: "{3F65F364-D34D-4D7D-AF0C-40AB9571D2F9}"
          operator: EQ
        }
      ]
    }, first : $first, after: $after
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
        categories: blogCategory {
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

// Replace internal CMS hostname with external one
const getPublicMediaUrl = (url: string) => {
  if (!url) return url;
  return url.replace('https://cm', 'https://xmcloudcm.localhost'); // Update with your external CMS hostname
};

interface FeaturedImage {
  value: ImageField
  src: string;
  alt: string;
}
interface ImageProps {
  field: FeaturedImage;
}

const Image = (props: ImageProps): JSX.Element => {
  const [src, setSrc] = useState(props.field.src);
  useEffect(() => {
    if (props.field?.value) {
      setSrc(getPublicMediaUrl(props.field?.src));
    }
  }, [props.field?.src]);

  return <JssImage field={{ ...props.field?.value, value: { ...props.field?.value, src } }} />;
};

export const Default = (props: BlogListingProps): JSX.Element => {
  const id = props.params.RenderingIdentifier;

  const [posts, setPosts] = useState<BlogPost[]>([]);

  useEffect(() => {
    const fetchPosts = async () => {
      try {
        const response = await graphQLClient.request(GET_BLOG_POSTS, {
          dataSource: props.rendering.dataSource,
          first: 10,
          after: "",
        });
        setPosts(response?.search?.results || []);
      } catch (error) {
        console.error('Error fetching blog posts:', error);
        setPosts([]);
      }
    };
    fetchPosts();
  }, [props.fields]);

  return (
    <div className={`component ${props.params.styles}`} id={id || undefined}>
      <div className="component-content px-3">
        <h1 className='blog-title text-center'>{props.fields.BlogListingTitle?.value}</h1>
        <p className='blog-description text-center'>{props.fields.BlogListingDescription?.value}</p>

        {posts.map((blog: BlogPost, index) => (
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
