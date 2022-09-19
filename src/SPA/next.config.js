const nextConfig = {
  reactStrictMode: true,
  swcMinify: true,
  loader: 'cloudinary',
  images: {
    unoptimized: true,
    loader: 'cloudinary',
    path: '',
  },
  trailingSlash: true,
  env: {
    NEXT_PUBLIC_TODO_HOST_BASE_URL: 'https://localhost:3001',
    NEXT_PUBLIC_TODO_API_BASE_URL: 'https://localhost:5001',
    NEXT_PUBLIC_AUTH0_ISSUER_BASE_URL: 'https://dev-ememplum.eu.auth0.com',
    NEXT_PUBLIC_AUTH0_CLIENT_ID: 'Q8wqFU7cZppYUp2natXkaSTMtSMlMWEL',
  },
};

module.exports = nextConfig;
