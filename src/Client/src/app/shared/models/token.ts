export interface Token {
    token: string;
    refreshToken: string;
    refreshTokenExpiryTime?: Date;
    permissions: string[];
}
