import React from 'react';
import {
  NextImage as JssImage,
  Link as JssLink,
  RichText as JssRichText,
  ImageField,
  Field,
  LinkField,
} from '@sitecore-jss/sitecore-jss-nextjs';

interface Fields {
  PromoIcon: ImageField;
  PromoText: Field<string>;
  PromoLink: LinkField;
  PromoText2: Field<string>;
}

type PromoProps = {
  params: { [key: string]: string };
  fields: Fields;
};

// Replace internal CMS hostname with external one
const getPublicMediaUrl = (url: string | undefined) => {
  if (!url) return url;
  return url.replace('https://cm', 'http://cm'); // Update with your external CMS hostname
};

const PromoDefaultComponent = (props: PromoProps): JSX.Element => (
  <div className={`component promo ${props?.params?.styles}`}>
    <div className="component-content">
      <span className="is-empty-hint">Promo</span>
    </div>
  </div>
);

interface PromoIconProps {
  promoIcon: ImageField;
}

const PromoIcon = ({ promoIcon }: PromoIconProps): JSX.Element => (
  <JssImage
    field={{
      ...promoIcon,
      value: {
        ...promoIcon?.value,
        src: getPublicMediaUrl(promoIcon?.value?.src),
      },
    }}
  />
);

export const Default = (props: PromoProps): JSX.Element => {
  const id = props.params.RenderingIdentifier;

  console.log('Promo image', getPublicMediaUrl(props.fields.PromoIcon?.value?.src));

  if (props.fields) {
    return (
      <div className={`component promo ${props?.params?.styles}`} id={id ? id : undefined}>
        <div className="component-content">
          <div className="field-promoicon">
            <PromoIcon promoIcon={props.fields.PromoIcon} />
          </div>
          <div className="promo-text">
            <div>
              <div className="field-promotext">
                <JssRichText field={props.fields.PromoText} />
              </div>
            </div>
            <div className="field-promolink">
              <JssLink field={props.fields.PromoLink} />
            </div>
          </div>
        </div>
      </div>
    );
  }

  return <PromoDefaultComponent {...props} />;
};

export const WithText = (props: PromoProps): JSX.Element => {
  const id = props.params.RenderingIdentifier;
  if (props.fields) {
    return (
      <div className={`component promo ${props.params.styles}`} id={id ? id : undefined}>
        <div className="component-content">
          <div className="field-promoicon">
            <JssImage field={props.fields.PromoIcon} />
          </div>
          <div className="promo-text">
            <div>
              <div className="field-promotext">
                <JssRichText className="promo-text" field={props.fields.PromoText} />
              </div>
            </div>
            <div className="field-promotext">
              <JssRichText className="promo-text" field={props.fields.PromoText2} />
            </div>
          </div>
        </div>
      </div>
    );
  }

  return <PromoDefaultComponent {...props} />;
};
