import { User } from "src/app/modules/identity/models/user";
import { UserService } from "src/app/modules/identity/services/user.service";
import { RoleService } from "src/app/modules/identity/services/role.service";
import { Role } from "src/app/modules/identity/models/role";
import { PickerConfiguration } from "src/app/shared/components/picker/picker-configuration";
import { FilterType } from "../components/filters/models/filter-type";

export const userPickerConfig: PickerConfiguration<User> = {
  service: UserService,
  name: x => x.lastName + ' ' + x.firstName,
  editor: { title: 'Seleziona un utente' },
  filters: [{
    type: FilterType.Text,
    name: 'search',
    placeholder: 'Cerca un utente',
    request: { specificField: 'fullName' }
  }]
};

export const usersPickerConfig: PickerConfiguration<User> = {
  ...userPickerConfig,
  editor: { title: 'Seleziona uno o più utenti' },
  multiple: true
};

export let rolesPickerConfig: PickerConfiguration<Role> = {
  service: RoleService,
  name: x => x.name,
  multiple: true,
  editor: { title: 'Seleziona uno o più ruoli' }
};