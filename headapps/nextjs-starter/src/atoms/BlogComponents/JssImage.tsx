import { Image as JssImage } from '@sitecore-jss/sitecore-jss-nextjs';
import { ImageProps } from 'components/BlogComponents/BlogListing';
import { getPublicMediaUrl } from '../ImageBaseUrlUtility';
import React, { useState, useEffect } from 'react';


export const Image = (props: ImageProps): JSX.Element => {
  const [src, setSrc] = useState(props.field.src);
  useEffect(() => {
    if (props.field?.value) {
      setSrc(getPublicMediaUrl(props.field?.src));
    }
  }, [props.field?.src]);

  return <JssImage field={{ ...props.field?.value, value: { ...props.field?.value, src } }} />;
};
