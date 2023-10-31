export interface Tenant {
  id: string;
  name: string;
  adminEmail: string;
  domain: string;
  connectionString?: string;
  isActive: boolean;
  endDate?: Date;
  issuer?: string;
}
