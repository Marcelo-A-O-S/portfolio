export type AuthTokenError =
    | "RefreshTokenInvalid"
    | "RefreshTokenMissing"
    | "RefreshAccessTokenError"
    | "RefreshTemporarilyUnavailable";