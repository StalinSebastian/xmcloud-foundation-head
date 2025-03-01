import React from 'react';
import { useEffect, useState } from 'react';

import {
  ComponentParams,
  ComponentRendering,
  Field,
  ImageField,
  Text,
  Image as JssImage,
  RichText,
} from '@sitecore-jss/sitecore-jss-nextjs';

interface Fields {
  BlogListingTitle: Field<string>;
  BlogListingDescription: Field<string>;
  items: BlogPostItem[];
}

interface BlogPostItem {
  id: Field<string>;
  fields: BlogPost;
}

interface BlogAuthorFields {
  fields: BlogAuthor;
}

interface BlogAuthor {
  BlogAuthorName: Field<string>;
  BlogAuthorPhoto: ImageField;
  BlogBlogAuthorBioTags: Field<string>;
}

interface BlogCategory {
  name: Field<string>;
  fields: BlogCategoryFields;
}

interface BlogCategoryFields {
  BlogCategoryName: Field<string>;
}

interface BlogTag {
  name: Field<string>;
  fields: BlogTagFields;
}

interface BlogTagFields {
  BlogTagName: Field<string>;
}

interface BlogPost {
  BlogHeading: Field<string>;
  BlogSummary: Field<string>;
  BlogImage: ImageField & { metadata?: { [key: string]: unknown } };
  BlogPublishDate: Field<string>;
  BlogAuthor: BlogAuthorFields;
  BlogCategory: BlogCategory[];
  BlogTags: BlogTag[];
}

interface BlogListingProps {
  rendering: ComponentRendering & { params: ComponentParams };
  params: ComponentParams;
  fields: Fields;
}

// Replace internal CMS hostname with external one
const getPublicMediaUrl = (url: string | undefined) => {
  if (!url) return url;
  return url.replace('https://cm', 'https://xmcloudcm.localhost'); // Update with your external CMS hostname
};

const Image = (props: ImageField): JSX.Element => {
  const [src, setSrc] = useState(props?.value?.src);

  useEffect(() => {
    if (props?.value?.src) {
      setSrc(getPublicMediaUrl(props?.value?.src));
    }
  }, [props?.value?.src]);

  return <JssImage field={{ ...props?.value, value: { ...props?.value, src } }} />;
};

export const Default = (props: BlogListingProps): JSX.Element => {
  const id = props.params.RenderingIdentifier;

  return (
    <div className={`component ${props.params.styles}`} id={id || undefined}>
      <div className="component-content">
        <p>BlogListing Component</p>

        <h1>{props.fields.BlogListingTitle?.value}</h1>
        <p>{props.fields.BlogListingDescription?.value}</p>

        {props.fields.items.map((item: BlogPostItem) => (
          <div key={item.id.value}>
            <Text tag="h2" field={item.fields.BlogHeading} />
            <Image field={item.fields.BlogImage} />
            <RichText tag="p" field={item.fields.BlogSummary} />
            <p>
              Publish date: <Text field={item.fields.BlogPublishDate} />
            </p>
            <p>
              Author: <Text tag="span" field={item.fields.BlogAuthor?.fields?.BlogAuthorName} />
            </p>
            <p>
              Category:&nbsp;
              {item.fields.BlogCategory?.map((category, index) => (
                <span key={index}>
                  {index > 0 ? ', ' : ''}
                  {category.fields.BlogCategoryName.value}
                </span>
              ))}
            </p>
            <p>
              Tags:&nbsp;
              {item.fields.BlogTags?.map((tag, index) => (
                <span key={index}>
                  {index > 0 ? ', ' : ''}
                  {tag.fields.BlogTagName.value}
                </span>
              ))}
            </p>

          </div>
        ))}
      </div>
    </div>
  );
};
