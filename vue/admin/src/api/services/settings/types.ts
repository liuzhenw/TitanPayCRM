export interface SettingGroupsDto {
  [key: string]: SettingItemDto[]
}

export interface SettingItemDto {
    name: string;
    displayName: string;
    description?: string;
    type: string;
    value?: string;
}

export interface SettingUpdateInput {
    name: string;
    value?: string;
}