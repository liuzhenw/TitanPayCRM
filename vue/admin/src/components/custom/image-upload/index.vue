<template>
  <el-upload class="cover-uploader" :show-file-list="false" :before-upload="onUpload">
    <div v-if="!cover" class="upload-placeholder">
      <ElIcon class="upload-icon"><Plus /></ElIcon>
      <div class="upload-text">点击上传图片</div>
    </div>
    <img v-else :src="cover" class="cover-image" />
  </el-upload>
</template>

<script setup lang="ts">
  import { StoreageService } from '@/api/storageApi'
  import { getImageExtensionFromMimeType } from '@/utils/file'
  import { ref } from 'vue'
  const { src, savePath, container, useRandomFileName, useCompressor } = defineProps({
    modelValue: {
      type: String,
      default: ''
    },
    src: {
      type: String,
      default: ''
    },
    savePath: {
      type: String,
      default: 'images'
    },
    container: {
      type: String,
      required: true
    },
    useRandomFileName: {
      type: Boolean,
      default: true
    },
    useCompressor: {
      type: Boolean,
      default: true
    }
  })
  const emit = defineEmits(['update:modelValue', 'success'])
  const cover = ref(src)
  const onUpload = async (file: File) => {
    let name = file.name
    if (useRandomFileName) {
      const ext = getImageExtensionFromMimeType(file)
      name = `${new Date().getTime().toString()}.${ext}`
    }
    const res = await StoreageService.uploadFile({
      file,
      container,
      fileName: `${savePath}/${name}`,
      useCompressor
    })
    cover.value = res.fileUrl
    emit('update:modelValue', res.fileName)
    return false // 阻止默认上传行为
  }

  watch(
    () => src,
    (val) => {
      cover.value = val
    }
  )
</script>

<style scoped lang="scss">
  .cover-uploader {
    position: relative;
    overflow: hidden;
    cursor: pointer;
    border-radius: 6px;
    transition: var(--el-transition-duration);
    &:hover {
      border-color: var(--el-color-primary);
    }

    .upload-placeholder {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      width: 260px;
      height: 160px;
      border: 1px dashed #d9d9d9;
      border-radius: 6px;

      .upload-icon {
        font-size: 28px;
        color: #8c939d;
      }

      .upload-text {
        margin-top: 8px;
        font-size: 14px;
        color: #8c939d;
      }
    }

    .cover-image {
      display: block;
      width: 260px;
      height: 160px;
      object-fit: cover;
      border: 1px dashed #d9d9d9;
      border-radius: 6px;
    }
  }

  .el-upload__tip {
    margin-top: 8px;
    font-size: 12px;
    color: #666;
  }
</style>
