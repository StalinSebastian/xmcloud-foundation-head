import React from 'react';
import { ComponentParams, ComponentRendering } from '@sitecore-jss/sitecore-jss-nextjs';

interface BlogDetailProps {
  rendering: ComponentRendering & { params: ComponentParams };
  params: ComponentParams;
}

export const Default = (props: BlogDetailProps): JSX.Element => {
  const id = props.params.RenderingIdentifier;

  return (
    <div className={`component ${props.params.styles}`} id={id || undefined}>
      <div className="component-content">
        <p>BlogDetail Component</p>
        <p>Fields: {JSON.stringify(props)}</p>
      </div>
    </div>
  );
};
