import React from 'react';
import {
  Field,
  ImageField,
  LinkField,
  ComponentParams,
  ComponentRendering,
  Link as JssLink,
  Image as JssImage,
} from '@sitecore-jss/sitecore-jss-nextjs';

interface Fields {
  TeaserHeading: Field<string>;
  TeaserSubHeading: Field<string>;
  TeaserContent: Field<string>;
  TeaserIcon: ImageField;
  TeaserLink: LinkField;
}

interface TextTeaserProps {
  rendering: ComponentRendering & { params: ComponentParams };
  params: ComponentParams;
  fields: Fields;
}

// Replace internal CMS hostname with external one
const getPublicMediaUrl = (url: string | undefined) => {
  if (!url) return url;
  return url.replace('https://cm', 'https://xmcloudcm.localhost'); // Update with your external CMS hostname
};

interface TeaserImageProps {
  teaserIcon: ImageField;
}

const TeaserImage = ({ teaserIcon }: TeaserImageProps): JSX.Element => {
  console.log('teaserIcon image', getPublicMediaUrl(teaserIcon?.value?.src));
  return (
    <JssImage
      field={{
        ...teaserIcon,
        value: {
          ...teaserIcon?.value,
          src: getPublicMediaUrl(teaserIcon?.value?.src),
        },
      }}
    />
  );
};

export const Default = (props: TextTeaserProps): JSX.Element => {
  const id = props.params.RenderingIdentifier;

  if (!props.fields) {
    return <></>;
  }

  const { TeaserHeading, TeaserSubHeading, TeaserContent, TeaserIcon, TeaserLink } = props.fields;

  return (
    <div className={`component text-teaser ${props.params.styles}`} id={id || undefined}>
      <div className="row component-content p-6 bg-gray-100 rounded-2xl shadow-lg">
        <div className="text-teaser col-12 col-md-4 d-flex flex-column align-items-center p-3">
          {TeaserIcon?.value && <TeaserImage teaserIcon={TeaserIcon} />}
        </div>
        <div className="text-teaser col-12 col-md-8 mt-4 d-flex flex-column">
          <h3 className="text-xl font-bold text-gray-800">{TeaserHeading?.value}</h3>
          <h4 className="text-lg text-gray-600 mt-1">{TeaserSubHeading?.value}</h4>
          <p className="text-gray-700 mt-3">{TeaserContent?.value}</p>
          {TeaserLink?.value && (
            <JssLink
              field={TeaserLink}
              className="mt-4 inline-block bg-blue-60 px-4 py-2 rounded-lg shadow-md hover:bg-blue-700"
            >
              Read More
            </JssLink>
          )}
        </div>
      </div>
    </div>
  );
};

export const WithoutImage = (props: TextTeaserProps): JSX.Element => {
  const id = props.params.RenderingIdentifier;

  if (!props.fields) {
    return <></>;
  }

  const { TeaserHeading, TeaserSubHeading, TeaserContent, TeaserLink } = props.fields;

  return (
    <div className={`component text-teaser p-5 ${props.params.styles}`} id={id || undefined}>
      <div className="row component-content p-6 bg-gray-100 rounded-2xl shadow-lg pt-5 pb-5">
        <div className="text-teaser col-12 col-md-6 d-flex flex-column align-items-center p-3">
          <h2 className="text-xl font-bold text-gray-800">{TeaserHeading?.value}</h2>
          <h3 className="text-lg text-gray-600 mt-1">{TeaserSubHeading?.value}</h3>
        </div>
        <div className="text-teaser col-12 col-md-6 mt-4 d-flex flex-column">
          <p className="text-gray-700 mt-3">{TeaserContent?.value}</p>
          {TeaserLink?.value && (
            <JssLink
              field={TeaserLink}
              className="mt-4 inline-block bg-blue-60 px-4 py-2 rounded-lg shadow-md hover:bg-blue-700"
            >
              Read More
            </JssLink>
          )}
        </div>
      </div>
    </div>
  );
};
