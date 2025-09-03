import request from '@/utils/http'

export interface FileUploadInput {
  file: File
  fileName: string
  container: string
  useCompressor: boolean
}
export interface FileUploadOutput {
  fileName: string
  fileUrl: string
  compressed: boolean
}

export class StoreageService {
  static async uploadFile(input: FileUploadInput) {
    const formData = new FormData()
    formData.append('file', input.file)
    formData.append('fileName', input.fileName)
    formData.append('container', input.container)
    formData.append('useCompressor', input.useCompressor.toString())
    return request.post<FileUploadOutput>({
      url: '/files/upload',
      data: formData
    })
  }

  static async downloadFile(url: string) {
    return request.get<string>({
      url,
      responseType: 'text'
    })
  }

  static async deleteFile(uri: string) {
    return request.del<void>({
      url: `/files/${uri}`
    })
  }
}
