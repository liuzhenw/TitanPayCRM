/* eslint-disable */
/* tslint:disable */
// @ts-nocheck
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface CrmAdminAccountsChangePasswordInput {
  /** @minLength 1 */
  oldPassword: string;
  /** @minLength 1 */
  newPassword: string;
}

export interface CrmAdminAccountsRoleDto {
  id?: string | null;
  name?: string | null;
  isStatic?: boolean;
  isPublic?: boolean;
  /** @format date-time */
  updatedAt?: string | null;
  /** @format date-time */
  createdAt?: string;
}

export interface CrmAdminAccountsUserBasicDto {
  /** @format uuid */
  id?: string;
  name?: string | null;
  email?: string | null;
  avatarUri?: string | null;
}

export interface CrmAdminAccountsUserDto {
  /** @format uuid */
  id?: string;
  name?: string | null;
  email?: string | null;
  avatarUri?: string | null;
  /** @format int32 */
  attempts?: number;
  /** @format date-time */
  lockedAt?: string | null;
  /** @format date-time */
  updatedAt?: string | null;
  /** @format date-time */
  createdAt?: string;
}

export interface CrmAdminAccountsUserWithDetailsDto {
  /** @format uuid */
  id?: string;
  name?: string | null;
  email?: string | null;
  avatarUri?: string | null;
  /** @format int32 */
  attempts?: number;
  /** @format date-time */
  lockedAt?: string | null;
  /** @format date-time */
  updatedAt?: string | null;
  /** @format date-time */
  createdAt?: string;
  roles?: string[] | null;
}

export interface CrmAdminProductsCreateProductInput {
  /**
   * @minLength 0
   * @maxLength 64
   */
  id: string;
  /**
   * @minLength 0
   * @maxLength 64
   */
  name: string;
  /**
   * @minLength 0
   * @maxLength 128
   */
  imageUri?: string | null;
  /**
   * @minLength 0
   * @maxLength 255
   */
  description?: string | null;
  /**
   * @format double
   * @min 0
   */
  price?: number;
}

export interface CrmAdminProductsProductBasicDto {
  id?: string | null;
  name?: string | null;
  imageUri?: string | null;
}

export interface CrmAdminProductsProductDto {
  id?: string | null;
  name?: string | null;
  imageUri?: string | null;
  description?: string | null;
  /** @format double */
  price?: number;
  /** @format int32 */
  salesVolume?: number;
  /** @format double */
  salesRevenue?: number;
  /** @format date-time */
  createdAt?: string;
  /** @format date-time */
  updatedAt?: string | null;
}

export interface CrmAdminProductsProductSaleLogDto {
  /** @format uuid */
  id?: string;
  product?: CrmAdminProductsProductBasicDto;
  customer?: CrmAdminAccountsUserBasicDto;
  orderNo?: string | null;
  /** @format int32 */
  quantity?: number;
  /** @format double */
  amount?: number;
  /** @format date-time */
  createdAt?: string;
}

export interface CrmAdminProductsUpdateProductInput {
  /**
   * @minLength 0
   * @maxLength 64
   */
  name: string;
  /**
   * @minLength 0
   * @maxLength 128
   */
  imageUri?: string | null;
  /**
   * @minLength 0
   * @maxLength 255
   */
  description?: string | null;
  /**
   * @format double
   * @min 0
   */
  price?: number;
}

export interface CrmAdminReferralsReferralLevelBasicDto {
  id?: string | null;
  name?: string | null;
  /** @format int32 */
  size?: number;
}

export interface CrmAdminReferralsReferralLevelCreateInput {
  /**
   * @minLength 1
   * @maxLength 32
   */
  id: string;
  /**
   * @minLength 1
   * @maxLength 32
   */
  name: string;
  /** @format int32 */
  size?: number;
  /**
   * @format double
   * @min 0
   */
  multiplier?: number;
}

export interface CrmAdminReferralsReferralLevelDto {
  id?: string | null;
  name?: string | null;
  /** @format int32 */
  size?: number;
  /** @format double */
  multiplier?: number;
  /** @format int32 */
  userCount?: number;
  /** @format double */
  totalCommission?: number;
  /** @format date-time */
  updatedAt?: string | null;
  /** @format date-time */
  createdAt?: string;
}

export interface CrmAdminReferralsReferralLevelUpdateInput {
  /**
   * @minLength 1
   * @maxLength 32
   */
  name: string;
  /** @format int32 */
  size?: number;
  /**
   * @format double
   * @min 0
   */
  multiplier?: number;
}

export interface CrmAdminReferralsReferrerDto {
  /** @format uuid */
  id?: string;
  user?: CrmAdminAccountsUserBasicDto;
  level?: CrmAdminReferralsReferralLevelBasicDto;
  /** @format int32 */
  directCount?: number;
  /** @format int32 */
  indirectCount?: number;
  /** @format int32 */
  totalCount?: number;
  /** @format double */
  commission?: number;
  /** @format double */
  withdrawal?: number;
  withdrawalAddress?: string | null;
  isDisabled?: boolean;
  remark?: string | null;
  /** @format date-time */
  updatedAt?: string | null;
  /** @format date-time */
  createdAt?: string;
}

export interface CrmAdminReferralsReferrerSaleStatisticDto {
  /** @format uuid */
  id?: string;
  product?: CrmAdminProductsProductBasicDto;
  /** @format int32 */
  volume?: number;
  /** @format double */
  revenue?: number;
  /** @format double */
  commission?: number;
  /** @format date-time */
  updatedAt?: string | null;
  /** @format date-time */
  createdAt?: string;
}

export interface CrmAdminReferralsReferrerUpdateInput {
  levelId?: string | null;
  isDisabled?: boolean;
  remark?: string | null;
}

export interface CrmAdminReferralsReferrerWithDetails {
  /** @format uuid */
  id?: string;
  user?: CrmAdminAccountsUserBasicDto;
  level?: CrmAdminReferralsReferralLevelBasicDto;
  /** @format int32 */
  directCount?: number;
  /** @format int32 */
  indirectCount?: number;
  /** @format int32 */
  totalCount?: number;
  /** @format double */
  commission?: number;
  /** @format double */
  withdrawal?: number;
  withdrawalAddress?: string | null;
  isDisabled?: boolean;
  remark?: string | null;
  /** @format date-time */
  updatedAt?: string | null;
  /** @format date-time */
  createdAt?: string;
  statistics?: CrmAdminReferralsReferrerSaleStatisticDto[] | null;
}

export interface CrmAdminSettingsSettingItemDto {
  name?: string | null;
  displayName?: string | null;
  description?: string | null;
  type?: string | null;
  value?: string | null;
}

export interface CrmAdminSettingsSettingUpdateInput {
  name?: string | null;
  value?: string | null;
}

export interface CrmSamplesSampleDto {
  /** @format int32 */
  value?: number;
}

export interface CrmServicesAuthAuthToken {
  accessToken?: string | null;
  /** @format int64 */
  expiresIn?: number;
}

export interface CrmServicesAuthCodeSignInInput {
  /**
   * @format email
   * @minLength 1
   */
  email: string;
  /** @minLength 1 */
  code: string;
}

export interface CrmServicesAuthPasswordSignInInput {
  /** @minLength 1 */
  userName: string;
  /** @minLength 1 */
  password: string;
}

export interface CrmServicesAuthSendEmailVerificationCodeInput {
  /**
   * @format email
   * @minLength 1
   */
  email: string;
}

export interface VoloAbpApplicationDtosPagedResultDto1CrmAdminAccountsUserDtoCrmAdminApplicationContractsVersion1000CultureNeutralPublicKeyTokenNull {
  items?: CrmAdminAccountsUserDto[] | null;
  /** @format int64 */
  totalCount?: number;
}

export interface VoloAbpApplicationDtosPagedResultDto1CrmAdminProductsProductSaleLogDtoCrmAdminApplicationContractsVersion1000CultureNeutralPublicKeyTokenNull {
  items?: CrmAdminProductsProductSaleLogDto[] | null;
  /** @format int64 */
  totalCount?: number;
}

export interface VoloAbpApplicationDtosPagedResultDto1CrmAdminReferralsReferrerDtoCrmAdminApplicationContractsVersion1000CultureNeutralPublicKeyTokenNull {
  items?: CrmAdminReferralsReferrerDto[] | null;
  /** @format int64 */
  totalCount?: number;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsApplicationAuthConfigurationDto {
  grantedPolicies?: Record<string, boolean>;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsApplicationConfigurationDto {
  localization?: VoloAbpAspNetCoreMvcApplicationConfigurationsApplicationLocalizationConfigurationDto;
  auth?: VoloAbpAspNetCoreMvcApplicationConfigurationsApplicationAuthConfigurationDto;
  setting?: VoloAbpAspNetCoreMvcApplicationConfigurationsApplicationSettingConfigurationDto;
  currentUser?: VoloAbpAspNetCoreMvcApplicationConfigurationsCurrentUserDto;
  features?: VoloAbpAspNetCoreMvcApplicationConfigurationsApplicationFeatureConfigurationDto;
  globalFeatures?: VoloAbpAspNetCoreMvcApplicationConfigurationsApplicationGlobalFeatureConfigurationDto;
  multiTenancy?: VoloAbpAspNetCoreMvcMultiTenancyMultiTenancyInfoDto;
  currentTenant?: VoloAbpAspNetCoreMvcMultiTenancyCurrentTenantDto;
  timing?: VoloAbpAspNetCoreMvcApplicationConfigurationsTimingDto;
  clock?: VoloAbpAspNetCoreMvcApplicationConfigurationsClockDto;
  objectExtensions?: VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingObjectExtensionsDto;
  extraProperties?: Record<string, any>;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsApplicationFeatureConfigurationDto {
  values?: Record<string, string | null>;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsApplicationGlobalFeatureConfigurationDto {
  /** @uniqueItems true */
  enabledFeatures?: string[] | null;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsApplicationLocalizationConfigurationDto {
  values?: Record<string, Record<string, string>>;
  resources?: Record<
    string,
    VoloAbpAspNetCoreMvcApplicationConfigurationsApplicationLocalizationResourceDto
  >;
  languages?: VoloAbpLocalizationLanguageInfo[] | null;
  currentCulture?: VoloAbpAspNetCoreMvcApplicationConfigurationsCurrentCultureDto;
  defaultResourceName?: string | null;
  languagesMap?: Record<string, VoloAbpNameValue[]>;
  languageFilesMap?: Record<string, VoloAbpNameValue[]>;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsApplicationLocalizationDto {
  resources?: Record<
    string,
    VoloAbpAspNetCoreMvcApplicationConfigurationsApplicationLocalizationResourceDto
  >;
  currentCulture?: VoloAbpAspNetCoreMvcApplicationConfigurationsCurrentCultureDto;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsApplicationLocalizationResourceDto {
  texts?: Record<string, string>;
  baseResources?: string[] | null;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsApplicationSettingConfigurationDto {
  values?: Record<string, string | null>;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsClockDto {
  kind?: string | null;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsCurrentCultureDto {
  displayName?: string | null;
  englishName?: string | null;
  threeLetterIsoLanguageName?: string | null;
  twoLetterIsoLanguageName?: string | null;
  isRightToLeft?: boolean;
  cultureName?: string | null;
  name?: string | null;
  nativeName?: string | null;
  dateTimeFormat?: VoloAbpAspNetCoreMvcApplicationConfigurationsDateTimeFormatDto;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsCurrentUserDto {
  isAuthenticated?: boolean;
  /** @format uuid */
  id?: string | null;
  /** @format uuid */
  tenantId?: string | null;
  /** @format uuid */
  impersonatorUserId?: string | null;
  /** @format uuid */
  impersonatorTenantId?: string | null;
  impersonatorUserName?: string | null;
  impersonatorTenantName?: string | null;
  userName?: string | null;
  name?: string | null;
  surName?: string | null;
  email?: string | null;
  emailVerified?: boolean;
  phoneNumber?: string | null;
  phoneNumberVerified?: boolean;
  roles?: string[] | null;
  sessionId?: string | null;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsDateTimeFormatDto {
  calendarAlgorithmType?: string | null;
  dateTimeFormatLong?: string | null;
  shortDatePattern?: string | null;
  fullDateTimePattern?: string | null;
  dateSeparator?: string | null;
  shortTimePattern?: string | null;
  longTimePattern?: string | null;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsIanaTimeZone {
  timeZoneName?: string | null;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingEntityExtensionDto {
  properties?: Record<
    string,
    VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyDto
  >;
  configuration?: Record<string, any>;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionEnumDto {
  fields?:
    | VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionEnumFieldDto[]
    | null;
  localizationResource?: string | null;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionEnumFieldDto {
  name?: string | null;
  value?: any;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyApiCreateDto {
  isAvailable?: boolean;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyApiDto {
  onGet?: VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyApiGetDto;
  onCreate?: VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyApiCreateDto;
  onUpdate?: VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyApiUpdateDto;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyApiGetDto {
  isAvailable?: boolean;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyApiUpdateDto {
  isAvailable?: boolean;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyAttributeDto {
  typeSimple?: string | null;
  config?: Record<string, any>;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyDto {
  type?: string | null;
  typeSimple?: string | null;
  displayName?: VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingLocalizableStringDto;
  api?: VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyApiDto;
  ui?: VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyUiDto;
  policy?: VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyPolicyDto;
  attributes?:
    | VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyAttributeDto[]
    | null;
  configuration?: Record<string, any>;
  defaultValue?: any;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyFeaturePolicyDto {
  features?: string[] | null;
  requiresAll?: boolean;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyGlobalFeaturePolicyDto {
  features?: string[] | null;
  requiresAll?: boolean;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyPermissionPolicyDto {
  permissionNames?: string[] | null;
  requiresAll?: boolean;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyPolicyDto {
  globalFeatures?: VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyGlobalFeaturePolicyDto;
  features?: VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyFeaturePolicyDto;
  permissions?: VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyPermissionPolicyDto;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyUiDto {
  onTable?: VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyUiTableDto;
  onCreateForm?: VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyUiFormDto;
  onEditForm?: VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyUiFormDto;
  lookup?: VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyUiLookupDto;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyUiFormDto {
  isVisible?: boolean;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyUiLookupDto {
  url?: string | null;
  resultListPropertyName?: string | null;
  displayPropertyName?: string | null;
  valuePropertyName?: string | null;
  filterParamName?: string | null;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionPropertyUiTableDto {
  isVisible?: boolean;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingLocalizableStringDto {
  name?: string | null;
  resource?: string | null;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingModuleExtensionDto {
  entities?: Record<
    string,
    VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingEntityExtensionDto
  >;
  configuration?: Record<string, any>;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingObjectExtensionsDto {
  modules?: Record<
    string,
    VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingModuleExtensionDto
  >;
  enums?: Record<
    string,
    VoloAbpAspNetCoreMvcApplicationConfigurationsObjectExtendingExtensionEnumDto
  >;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsTimeZone {
  iana?: VoloAbpAspNetCoreMvcApplicationConfigurationsIanaTimeZone;
  windows?: VoloAbpAspNetCoreMvcApplicationConfigurationsWindowsTimeZone;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsTimingDto {
  timeZone?: VoloAbpAspNetCoreMvcApplicationConfigurationsTimeZone;
}

export interface VoloAbpAspNetCoreMvcApplicationConfigurationsWindowsTimeZone {
  timeZoneId?: string | null;
}

export interface VoloAbpAspNetCoreMvcMultiTenancyCurrentTenantDto {
  /** @format uuid */
  id?: string | null;
  name?: string | null;
  isAvailable?: boolean;
}

export interface VoloAbpAspNetCoreMvcMultiTenancyMultiTenancyInfoDto {
  isEnabled?: boolean;
}

export interface VoloAbpHttpModelingActionApiDescriptionModel {
  uniqueName?: string | null;
  name?: string | null;
  httpMethod?: string | null;
  url?: string | null;
  supportedVersions?: string[] | null;
  parametersOnMethod?:
    | VoloAbpHttpModelingMethodParameterApiDescriptionModel[]
    | null;
  parameters?: VoloAbpHttpModelingParameterApiDescriptionModel[] | null;
  returnValue?: VoloAbpHttpModelingReturnValueApiDescriptionModel;
  allowAnonymous?: boolean | null;
  implementFrom?: string | null;
}

export interface VoloAbpHttpModelingApplicationApiDescriptionModel {
  modules?: Record<string, VoloAbpHttpModelingModuleApiDescriptionModel>;
  types?: Record<string, VoloAbpHttpModelingTypeApiDescriptionModel>;
}

export interface VoloAbpHttpModelingControllerApiDescriptionModel {
  controllerName?: string | null;
  controllerGroupName?: string | null;
  isRemoteService?: boolean;
  isIntegrationService?: boolean;
  apiVersion?: string | null;
  type?: string | null;
  interfaces?:
    | VoloAbpHttpModelingControllerInterfaceApiDescriptionModel[]
    | null;
  actions?: Record<string, VoloAbpHttpModelingActionApiDescriptionModel>;
}

export interface VoloAbpHttpModelingControllerInterfaceApiDescriptionModel {
  type?: string | null;
  name?: string | null;
  methods?: VoloAbpHttpModelingInterfaceMethodApiDescriptionModel[] | null;
}

export interface VoloAbpHttpModelingInterfaceMethodApiDescriptionModel {
  name?: string | null;
  parametersOnMethod?:
    | VoloAbpHttpModelingMethodParameterApiDescriptionModel[]
    | null;
  returnValue?: VoloAbpHttpModelingReturnValueApiDescriptionModel;
}

export interface VoloAbpHttpModelingMethodParameterApiDescriptionModel {
  name?: string | null;
  typeAsString?: string | null;
  type?: string | null;
  typeSimple?: string | null;
  isOptional?: boolean;
  defaultValue?: any;
}

export interface VoloAbpHttpModelingModuleApiDescriptionModel {
  rootPath?: string | null;
  remoteServiceName?: string | null;
  controllers?: Record<
    string,
    VoloAbpHttpModelingControllerApiDescriptionModel
  >;
}

export interface VoloAbpHttpModelingParameterApiDescriptionModel {
  nameOnMethod?: string | null;
  name?: string | null;
  jsonName?: string | null;
  type?: string | null;
  typeSimple?: string | null;
  isOptional?: boolean;
  defaultValue?: any;
  constraintTypes?: string[] | null;
  bindingSourceId?: string | null;
  descriptorName?: string | null;
}

export interface VoloAbpHttpModelingPropertyApiDescriptionModel {
  name?: string | null;
  jsonName?: string | null;
  type?: string | null;
  typeSimple?: string | null;
  isRequired?: boolean;
  /** @format int32 */
  minLength?: number | null;
  /** @format int32 */
  maxLength?: number | null;
  minimum?: string | null;
  maximum?: string | null;
  regex?: string | null;
}

export interface VoloAbpHttpModelingReturnValueApiDescriptionModel {
  type?: string | null;
  typeSimple?: string | null;
}

export interface VoloAbpHttpModelingTypeApiDescriptionModel {
  baseType?: string | null;
  isEnum?: boolean;
  enumNames?: string[] | null;
  enumValues?: any[] | null;
  genericArguments?: string[] | null;
  properties?: VoloAbpHttpModelingPropertyApiDescriptionModel[] | null;
}

export interface VoloAbpHttpRemoteServiceErrorInfo {
  code?: string | null;
  message?: string | null;
  details?: string | null;
  data?: Record<string, any>;
  validationErrors?: VoloAbpHttpRemoteServiceValidationErrorInfo[] | null;
}

export interface VoloAbpHttpRemoteServiceErrorResponse {
  error?: VoloAbpHttpRemoteServiceErrorInfo;
}

export interface VoloAbpHttpRemoteServiceValidationErrorInfo {
  message?: string | null;
  members?: string[] | null;
}

export interface VoloAbpLocalizationLanguageInfo {
  cultureName?: string | null;
  uiCultureName?: string | null;
  displayName?: string | null;
  twoLetterISOLanguageName?: string | null;
}

export interface VoloAbpNameValue {
  name?: string | null;
  value?: string | null;
}

export type QueryParamsType = Record<string | number, any>;
export type ResponseFormat = keyof Omit<Body, "body" | "bodyUsed">;

export interface FullRequestParams extends Omit<RequestInit, "body"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseFormat;
  /** request body */
  body?: unknown;
  /** base url */
  baseUrl?: string;
  /** request cancellation token */
  cancelToken?: CancelToken;
}

export type RequestParams = Omit<
  FullRequestParams,
  "body" | "method" | "query" | "path"
>;

export interface ApiConfig<SecurityDataType = unknown> {
  baseUrl?: string;
  baseApiParams?: Omit<RequestParams, "baseUrl" | "cancelToken" | "signal">;
  securityWorker?: (
    securityData: SecurityDataType | null,
  ) => Promise<RequestParams | void> | RequestParams | void;
  customFetch?: typeof fetch;
}

export interface HttpResponse<D extends unknown, E extends unknown = unknown>
  extends Response {
  data: D;
  error: E;
}

type CancelToken = Symbol | string | number;

export enum ContentType {
  Json = "application/json",
  JsonApi = "application/vnd.api+json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
  Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
  public baseUrl: string = "";
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private abortControllers = new Map<CancelToken, AbortController>();
  private customFetch = (...fetchParams: Parameters<typeof fetch>) =>
    fetch(...fetchParams);

  private baseApiParams: RequestParams = {
    credentials: "same-origin",
    headers: {},
    redirect: "follow",
    referrerPolicy: "no-referrer",
  };

  constructor(apiConfig: ApiConfig<SecurityDataType> = {}) {
    Object.assign(this, apiConfig);
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected encodeQueryParam(key: string, value: any) {
    const encodedKey = encodeURIComponent(key);
    return `${encodedKey}=${encodeURIComponent(typeof value === "number" ? value : `${value}`)}`;
  }

  protected addQueryParam(query: QueryParamsType, key: string) {
    return this.encodeQueryParam(key, query[key]);
  }

  protected addArrayQueryParam(query: QueryParamsType, key: string) {
    const value = query[key];
    return value.map((v: any) => this.encodeQueryParam(key, v)).join("&");
  }

  protected toQueryString(rawQuery?: QueryParamsType): string {
    const query = rawQuery || {};
    const keys = Object.keys(query).filter(
      (key) => "undefined" !== typeof query[key],
    );
    return keys
      .map((key) =>
        Array.isArray(query[key])
          ? this.addArrayQueryParam(query, key)
          : this.addQueryParam(query, key),
      )
      .join("&");
  }

  protected addQueryParams(rawQuery?: QueryParamsType): string {
    const queryString = this.toQueryString(rawQuery);
    return queryString ? `?${queryString}` : "";
  }

  private contentFormatters: Record<ContentType, (input: any) => any> = {
    [ContentType.Json]: (input: any) =>
      input !== null && (typeof input === "object" || typeof input === "string")
        ? JSON.stringify(input)
        : input,
    [ContentType.JsonApi]: (input: any) =>
      input !== null && (typeof input === "object" || typeof input === "string")
        ? JSON.stringify(input)
        : input,
    [ContentType.Text]: (input: any) =>
      input !== null && typeof input !== "string"
        ? JSON.stringify(input)
        : input,
    [ContentType.FormData]: (input: any) => {
      if (input instanceof FormData) {
        return input;
      }

      return Object.keys(input || {}).reduce((formData, key) => {
        const property = input[key];
        formData.append(
          key,
          property instanceof Blob
            ? property
            : typeof property === "object" && property !== null
              ? JSON.stringify(property)
              : `${property}`,
        );
        return formData;
      }, new FormData());
    },
    [ContentType.UrlEncoded]: (input: any) => this.toQueryString(input),
  };

  protected mergeRequestParams(
    params1: RequestParams,
    params2?: RequestParams,
  ): RequestParams {
    return {
      ...this.baseApiParams,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...(this.baseApiParams.headers || {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected createAbortSignal = (
    cancelToken: CancelToken,
  ): AbortSignal | undefined => {
    if (this.abortControllers.has(cancelToken)) {
      const abortController = this.abortControllers.get(cancelToken);
      if (abortController) {
        return abortController.signal;
      }
      return void 0;
    }

    const abortController = new AbortController();
    this.abortControllers.set(cancelToken, abortController);
    return abortController.signal;
  };

  public abortRequest = (cancelToken: CancelToken) => {
    const abortController = this.abortControllers.get(cancelToken);

    if (abortController) {
      abortController.abort();
      this.abortControllers.delete(cancelToken);
    }
  };

  public request = async <T = any, E = any>({
    body,
    secure,
    path,
    type,
    query,
    format,
    baseUrl,
    cancelToken,
    ...params
  }: FullRequestParams): Promise<HttpResponse<T, E>> => {
    const secureParams =
      ((typeof secure === "boolean" ? secure : this.baseApiParams.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const queryString = query && this.toQueryString(query);
    const payloadFormatter = this.contentFormatters[type || ContentType.Json];
    const responseFormat = format || requestParams.format;

    return this.customFetch(
      `${baseUrl || this.baseUrl || ""}${path}${queryString ? `?${queryString}` : ""}`,
      {
        ...requestParams,
        headers: {
          ...(requestParams.headers || {}),
          ...(type && type !== ContentType.FormData
            ? { "Content-Type": type }
            : {}),
        },
        signal:
          (cancelToken
            ? this.createAbortSignal(cancelToken)
            : requestParams.signal) || null,
        body:
          typeof body === "undefined" || body === null
            ? null
            : payloadFormatter(body),
      },
    ).then(async (response) => {
      const r = response as HttpResponse<T, E>;
      r.data = null as unknown as T;
      r.error = null as unknown as E;

      const data = !responseFormat
        ? r
        : await response[responseFormat]()
            .then((data) => {
              if (r.ok) {
                r.data = data;
              } else {
                r.error = data;
              }
              return r;
            })
            .catch((e) => {
              r.error = e;
              return r;
            });

      if (cancelToken) {
        this.abortControllers.delete(cancelToken);
      }

      if (!response.ok) throw data;
      return data;
    });
  };
}

/**
 * @title Crm.Api
 * @version v1
 *
 * Crm.Api
 */
export class Api<
  SecurityDataType extends unknown,
> extends HttpClient<SecurityDataType> {
  api = {
    /**
     * No description
     *
     * @tags Account
     * @name AdminAccountList
     * @request GET:/api/admin/account
     */
    adminAccountList: (params: RequestParams = {}) =>
      this.request<CrmAdminAccountsUserWithDetailsDto, any>({
        path: `/api/admin/account`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Account
     * @name AdminAccountPasswordUpdate
     * @request PUT:/api/admin/account/password
     */
    adminAccountPasswordUpdate: (
      data: CrmAdminAccountsChangePasswordInput,
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/admin/account/password`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Auth
     * @name AuthSignInPasswordCreate
     * @request POST:/api/auth/sign-in/password
     */
    authSignInPasswordCreate: (
      data: CrmServicesAuthPasswordSignInInput,
      params: RequestParams = {},
    ) =>
      this.request<CrmServicesAuthAuthToken, any>({
        path: `/api/auth/sign-in/password`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Auth
     * @name AuthSignInCodeCreate
     * @request POST:/api/auth/sign-in/code
     */
    authSignInCodeCreate: (
      data: CrmServicesAuthCodeSignInInput,
      params: RequestParams = {},
    ) =>
      this.request<CrmServicesAuthAuthToken, any>({
        path: `/api/auth/sign-in/code`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Auth
     * @name AuthVerificationCodeCreate
     * @request POST:/api/auth/verification-code
     */
    authVerificationCodeCreate: (
      data: CrmServicesAuthSendEmailVerificationCodeInput,
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/auth/verification-code`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Example
     * @name ApiCrmExampleList
     * @request GET:/api/api/crm/example
     */
    apiCrmExampleList: (params: RequestParams = {}) =>
      this.request<CrmSamplesSampleDto, VoloAbpHttpRemoteServiceErrorResponse>({
        path: `/api/api/crm/example`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Example
     * @name ApiCrmExampleAuthorizedList
     * @request GET:/api/api/crm/example/authorized
     */
    apiCrmExampleAuthorizedList: (params: RequestParams = {}) =>
      this.request<CrmSamplesSampleDto, VoloAbpHttpRemoteServiceErrorResponse>({
        path: `/api/api/crm/example/authorized`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Product
     * @name AdminProductsDetail
     * @request GET:/api/admin/products/{id}
     */
    adminProductsDetail: (id: string, params: RequestParams = {}) =>
      this.request<CrmAdminProductsProductDto, any>({
        path: `/api/admin/products/${id}`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Product
     * @name AdminProductsUpdate
     * @request PUT:/api/admin/products/{id}
     */
    adminProductsUpdate: (
      id: string,
      data: CrmAdminProductsUpdateProductInput,
      params: RequestParams = {},
    ) =>
      this.request<CrmAdminProductsProductDto, any>({
        path: `/api/admin/products/${id}`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Product
     * @name AdminProductsDelete
     * @request DELETE:/api/admin/products/{id}
     */
    adminProductsDelete: (id: string, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/admin/products/${id}`,
        method: "DELETE",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Product
     * @name AdminProductsList
     * @request GET:/api/admin/products
     */
    adminProductsList: (params: RequestParams = {}) =>
      this.request<CrmAdminProductsProductDto[], any>({
        path: `/api/admin/products`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Product
     * @name AdminProductsCreate
     * @request POST:/api/admin/products
     */
    adminProductsCreate: (
      data: CrmAdminProductsCreateProductInput,
      params: RequestParams = {},
    ) =>
      this.request<CrmAdminProductsProductDto, any>({
        path: `/api/admin/products`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags ProductSaleLog
     * @name AdminProductsSaleLogsList
     * @request GET:/api/admin/products/sale-logs
     */
    adminProductsSaleLogsList: (
      query?: {
        ProductId?: string;
        /** @format uuid */
        CustomerId?: string;
        OrderNo?: string;
        Sorting?: string;
        /**
         * @format int32
         * @min 0
         * @max 2147483647
         */
        SkipCount?: number;
        /**
         * @format int32
         * @min 1
         * @max 2147483647
         */
        MaxResultCount?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<
        VoloAbpApplicationDtosPagedResultDto1CrmAdminProductsProductSaleLogDtoCrmAdminApplicationContractsVersion1000CultureNeutralPublicKeyTokenNull,
        any
      >({
        path: `/api/admin/products/sale-logs`,
        method: "GET",
        query: query,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags ReferralLevel
     * @name AdminReferralsLevelsList
     * @request GET:/api/admin/referrals/levels
     */
    adminReferralsLevelsList: (params: RequestParams = {}) =>
      this.request<CrmAdminReferralsReferralLevelDto[], any>({
        path: `/api/admin/referrals/levels`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags ReferralLevel
     * @name AdminReferralsLevelsCreate
     * @request POST:/api/admin/referrals/levels
     */
    adminReferralsLevelsCreate: (
      data: CrmAdminReferralsReferralLevelCreateInput,
      params: RequestParams = {},
    ) =>
      this.request<CrmAdminReferralsReferralLevelDto, any>({
        path: `/api/admin/referrals/levels`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags ReferralLevel
     * @name AdminReferralsLevelsUpdate
     * @request PUT:/api/admin/referrals/levels/{id}
     */
    adminReferralsLevelsUpdate: (
      id: string,
      data: CrmAdminReferralsReferralLevelUpdateInput,
      params: RequestParams = {},
    ) =>
      this.request<CrmAdminReferralsReferralLevelDto, any>({
        path: `/api/admin/referrals/levels/${id}`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags ReferralLevel
     * @name AdminReferralsLevelsDelete
     * @request DELETE:/api/admin/referrals/levels/{id}
     */
    adminReferralsLevelsDelete: (id: string, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/admin/referrals/levels/${id}`,
        method: "DELETE",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Referrer
     * @name AdminReferralsReferrersDetail
     * @request GET:/api/admin/referrals/referrers/{id}
     */
    adminReferralsReferrersDetail: (id: string, params: RequestParams = {}) =>
      this.request<CrmAdminReferralsReferrerWithDetails, any>({
        path: `/api/admin/referrals/referrers/${id}`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Referrer
     * @name AdminReferralsReferrersUpdate
     * @request PUT:/api/admin/referrals/referrers/{id}
     */
    adminReferralsReferrersUpdate: (
      id: string,
      data: CrmAdminReferralsReferrerUpdateInput,
      params: RequestParams = {},
    ) =>
      this.request<CrmAdminReferralsReferrerWithDetails, any>({
        path: `/api/admin/referrals/referrers/${id}`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Referrer
     * @name AdminReferralsReferrersList
     * @request GET:/api/admin/referrals/referrers
     */
    adminReferralsReferrersList: (
      query?: {
        /** @format uuid */
        Id?: string;
        LevelId?: string;
        Sorting?: string;
        /**
         * @format int32
         * @min 0
         * @max 2147483647
         */
        SkipCount?: number;
        /**
         * @format int32
         * @min 1
         * @max 2147483647
         */
        MaxResultCount?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<
        VoloAbpApplicationDtosPagedResultDto1CrmAdminReferralsReferrerDtoCrmAdminApplicationContractsVersion1000CultureNeutralPublicKeyTokenNull,
        any
      >({
        path: `/api/admin/referrals/referrers`,
        method: "GET",
        query: query,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Role
     * @name AdminAccountsRolesList
     * @request GET:/api/admin/accounts/roles
     */
    adminAccountsRolesList: (params: RequestParams = {}) =>
      this.request<CrmAdminAccountsRoleDto[], any>({
        path: `/api/admin/accounts/roles`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Setting
     * @name AdminSettingsDetail
     * @request GET:/api/admin/settings/{name}
     */
    adminSettingsDetail: (name: string, params: RequestParams = {}) =>
      this.request<CrmAdminSettingsSettingItemDto, any>({
        path: `/api/admin/settings/${name}`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Setting
     * @name AdminSettingsList
     * @request GET:/api/admin/settings
     */
    adminSettingsList: (params: RequestParams = {}) =>
      this.request<Record<string, CrmAdminSettingsSettingItemDto[]>, any>({
        path: `/api/admin/settings`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Setting
     * @name AdminSettingsUpdate
     * @request PUT:/api/admin/settings
     */
    adminSettingsUpdate: (
      data: CrmAdminSettingsSettingUpdateInput,
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/admin/settings`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags User
     * @name AdminAccountsUsersDetail
     * @request GET:/api/admin/accounts/users/{id}
     */
    adminAccountsUsersDetail: (id: string, params: RequestParams = {}) =>
      this.request<CrmAdminAccountsUserWithDetailsDto, any>({
        path: `/api/admin/accounts/users/${id}`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags User
     * @name AdminAccountsUsersList
     * @request GET:/api/admin/accounts/users
     */
    adminAccountsUsersList: (
      query?: {
        Email?: string;
        Name?: string;
        Sorting?: string;
        /**
         * @format int32
         * @min 0
         * @max 2147483647
         */
        SkipCount?: number;
        /**
         * @format int32
         * @min 1
         * @max 2147483647
         */
        MaxResultCount?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<
        VoloAbpApplicationDtosPagedResultDto1CrmAdminAccountsUserDtoCrmAdminApplicationContractsVersion1000CultureNeutralPublicKeyTokenNull,
        any
      >({
        path: `/api/admin/accounts/users`,
        method: "GET",
        query: query,
        format: "json",
        ...params,
      }),
  };
}
