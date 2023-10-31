import { RoleService } from "../services/role.service";
import { Role } from "./role";
import { FilterType } from "src/app/shared/components/filters/models/filter-type";
import { Filter } from "src/app/shared/components/filters/models/filter";

export const filters: Filter[] = [
  {
    type: FilterType.Text,
    name: 'search',
    placeholder: 'Cerca un utente',
    request: { specificField: 'fullName' }
  },
  {
    type: FilterType.Autocomplete,
    name: 'roles',
    placeholder: 'Tutti i ruoli',
    config: { service: RoleService, label: (x: Role) => x.name, filter: { specificField: 'roleId' } },
    request: { specificField: 'roleId' }
  },
  {
    type: FilterType.Autocomplete,
    name: 'status',
    placeholder: 'Tutti gli stati',
    values: [{ id: 'true', value: 'Abilitato' }, { id: 'false', value: 'Disabilitato' }],
    request: { field: 'IsActive', operator: 'eq', value: 'true' }
  },
];

