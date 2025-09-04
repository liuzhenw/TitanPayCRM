export interface ProductBasicDto {
  id: string
  name: string
  imageUrl?: string
}
export interface ProductDto extends ProductBasicDto {
  description?: string
  imageUri?: string
  price: number
  salesVolume: number
  salesRevenue: number
  createdAt: string
  updatedAt?: string
}

export interface ProductCreateInput {
  id: string
  name: string
  description?: string
  imageUri?: string
  price: number
}

export interface ProductUpdateInput {
  name: string
  description?: string
  price: number
  imageUri?: string
}
