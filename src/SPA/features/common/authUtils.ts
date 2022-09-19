const roleNamespace = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

export const hasRole = (claims: any, role: string) => {
  if (claims && Array.isArray(claims[roleNamespace]) && claims[roleNamespace].includes(role)) {
    return true;
  }
  return false;
};
