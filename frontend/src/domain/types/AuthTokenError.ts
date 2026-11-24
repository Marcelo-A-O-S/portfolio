export type AuthTokenError =
    | "RefreshTokenMissing"
    | "RefreshAccessTokenError"
    | "RefreshTemporarilyUnavailable";