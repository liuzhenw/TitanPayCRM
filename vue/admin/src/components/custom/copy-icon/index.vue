<template>
  <i class="iconfont-sys iconsys-copy" style="cursor: pointer" @click="onCopy"></i>
</template>

<script setup lang="ts">
  import { ElMessage } from 'element-plus'
  const { content, showMessage, message } = defineProps({
    content: {
      type: String,
      default: ''
    },
    showMessage: {
      type: Boolean,
      default: true
    },
    message: {
      type: String,
      default: '复制成功'
    }
  })
  const emit = defineEmits(['copy'])
  const onCopy = async () => {
    if (navigator.clipboard) {
      try {
        await navigator.clipboard.writeText(content)
        console.log('Text copied to clipboard')
        emit('copy')
        if (showMessage) {
          ElMessage.success(message)
        }
      } catch (err) {
        console.error('Failed to copy text: ', err)
      }
      return
    }

    // Fallback for browsers that do not support the Clipboard API
    const textArea = document.createElement('textarea')
    textArea.value = content
    textArea.style.position = 'fixed' // Prevent scrolling to bottom of page in MS Edge
    textArea.style.opacity = '0' // Make it invisible
    document.body.appendChild(textArea)
    textArea.focus()
    textArea.select()
    try {
      const successful = document.execCommand('copy')
      if (successful) {
        console.log('Text copied to clipboard')
        emit('copy', content)
      } else {
        console.error('Failed to copy text')
      }
    } catch (err) {
      console.error('Failed to copy text: ', err)
    } finally {
      document.body.removeChild(textArea)
    }
  }
</script>

<style scoped lang="scss"></style>
