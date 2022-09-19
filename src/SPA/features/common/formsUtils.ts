import { toCamelCase } from './stringUtils';

/**
 * Handles validation errors when submitting forms.  The submission function will be called
 * and if a validation error is thrown the errors will be mapped to the form errors
 */
export const formSubmissionHandler = async <T>(
  submission: () => Promise<void>,
  setFieldError: (field: keyof T, error: React.ReactNode) => void
) => {
  try {
    await submission();
  } catch (error: any) {
    if (error?.response.status === 400) {
      const validationErrors = error.response.data.errors;

      Object.keys(validationErrors).forEach((key) => {
        const localKey = toCamelCase(key) as keyof T;
        if (localKey) {
          setFieldError(localKey, validationErrors[key][0]);
        }
      });
    }
  }
};
