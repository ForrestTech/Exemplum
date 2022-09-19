// Add this rule to auth0 to make sure that user roles are added to access tokens.  This supports
// asp.net role based authentication when calling the API.

function (user, context, callback) {
  const namespace = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
  const permissions = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/permissions';
  const assignedRoles = (context.authorization || {}).roles;
  const assignedPermissions = (context.authorization || {}).permissions;
  
  let accessTokenClaims = context.accessToken || {}; 
  accessTokenClaims[`${namespace}`] = assignedRoles;
  accessTokenClaims[`${permissions}`] = assignedPermissions;
  context.accessToken = accessTokenClaims;
  
  let idTokenClaims = context.idToken || {}; 
  idTokenClaims[`${namespace}`] = assignedRoles;
  idTokenClaims[`${permissions}`] = assignedPermissions;
  context.idToken = idTokenClaims;

  callback(null, user, context);
}