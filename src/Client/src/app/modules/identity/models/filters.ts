import { RoleService } from "../services/role.service";
import { Role } from "./role";
import { FilterType } from "src/app/shared/components/filters/models/filter-type";
import { Filter } from "src/app/shared/components/filters/models/filter";

export const filters: Filter[] = [
  {
    type: FilterType.Text,
    label: 'filter.nameSearch',
    request: { field: '@fullName' }
  },
  {
    type: FilterType.Select,
    label: 'Tutti i ruoli',
    select: { 
      service: RoleService, 
      label: (x: Role) => x.name, 
      request: { field: '@roleId' } 
    },
    request: { field: '@roleId' }
  },
  {
    type: FilterType.Select,
    label: 'Tutti gli stati',
    select: {
      options: [
        { value: true, label: 'Abilitato' }, 
        { value: false, label: 'Disabilitato' }
      ]
    },
    request: { field: 'isActive', operator: 'eq', value: true }
  },
];

