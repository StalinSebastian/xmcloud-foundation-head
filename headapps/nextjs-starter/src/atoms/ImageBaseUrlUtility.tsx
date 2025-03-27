
// Replace internal CMS hostname with external one

export const getPublicMediaUrl = (url: string) => {
  if (!url) return url;
  return url.replace('https://cm', 'https://xmcloudcm.localhost'); // Update with your external CMS hostname
};
