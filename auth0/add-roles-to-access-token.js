// Add this rule to auth0 to make sure that user roles are added to access tokens.  This supports
// asp.net role based authentication when calling the API.

function (user, context, callback) {
  const namespace = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
  const assignedRoles = (context.authorization || {}).roles;

  let accessTokenClaims = context.accessToken || {};

  accessTokenClaims[`${namespace}`] = assignedRoles;

  context.accessToken = accessTokenClaims;

  callback(null, user, context);
}