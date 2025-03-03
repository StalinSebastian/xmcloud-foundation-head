import React from 'react';
import { ComponentParams, ComponentRendering, Placeholder } from '@sitecore-jss/sitecore-jss-nextjs';

interface BlogListingContainerProps {
  rendering: ComponentRendering & { params: ComponentParams };
  params: ComponentParams;
}

export const Default = (props: BlogListingContainerProps): JSX.Element => {
  const id = props.params.RenderingIdentifier;

  return (
    <div className={`component blog-listing-container ${props.params.styles}`} id={id ? id : undefined}>
      <div className="component-content row">
        <div className="column-left">
          <div className="row">
            <Placeholder name="column-left" rendering={props.rendering} />
          </div>
        </div>
        <div className="column-right">
          <div className="row">
            <Placeholder name="column-right" rendering={props.rendering} />
          </div>
        </div>
      </div>
    </div>
  );
};
