export function getImageExtensionFromMimeType(file: File): string | null {
  const mimeTypeToExt: Record<string, string> = {
    'image/jpeg': 'jpg',
    'image/png': 'png',
    'image/gif': 'gif',
    'image/bmp': 'bmp',
    'image/webp': 'webp',
    'image/svg+xml': 'svg',
    'image/tiff': 'tiff',
    'image/x-icon': 'ico',
    'image/heic': 'heic'
  }

  return mimeTypeToExt[file.type] || null
}
