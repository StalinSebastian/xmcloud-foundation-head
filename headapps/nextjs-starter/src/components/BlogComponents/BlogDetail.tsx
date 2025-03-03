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
  Id: Field<string>;
  BlogSummary: Field<string>;
  BlogHeading: Field<string>;
  BlogContent: Field<string>;
  BlogImage: ImageField & { metadata?: { [key: string]: unknown } };
  BlogPublishDate: Field<string>;
  BlogAuthor: BlogAuthorFields;
  BlogCategory: BlogCategory[];
  BlogTags: BlogTag[];
}

interface BlogDetailProps {
  rendering: ComponentRendering & { params: ComponentParams };
  params: ComponentParams;
  fields: BlogPost;
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

export const Default = (props: BlogDetailProps): JSX.Element => {
  const id = props.params.RenderingIdentifier;

  return (
    <div className={`component ${props.params.styles}`} id={id || undefined}>
      <div className="component-content">
        <p>BlogDetail Component</p>
        <div key={props.fields.Id.value}>
          <Text tag="h2" field={props.fields.BlogHeading} />
          <Image field={props.fields.BlogImage} />
          <RichText tag="p" field={props.fields.BlogSummary} />
          <p>
            Publish date: <Text field={props.fields.BlogPublishDate} />
          </p>
          <p>
            Author: <Text tag="span" field={props.fields.BlogAuthor?.fields?.BlogAuthorName} />
          </p>
          <p>
            Category:&nbsp;
            {props.fields.BlogCategory?.map((category, index) => (
              <span key={index}>
                {index > 0 ? ', ' : ''}
                {category.fields.BlogCategoryName.value}
              </span>
            ))}
          </p>
          <p>
            Tags:&nbsp;
            {props.fields.BlogTags?.map((tag, index) => (
              <span key={index}>
                {index > 0 ? ', ' : ''}
                {tag.fields.BlogTagName.value}
              </span>
            ))}
          </p>
        </div>
      </div>
    </div>
  );
};
