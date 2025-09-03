<template>
  <el-tooltip
    :content="value"
    placement="top"
    effect="dark"
    :show-after="500"
    :disabled="!isEllipsis"
  >
    <el-space>
      <el-text :type="textType">{{ display }}</el-text>
      <CopyIcon v-if="copy && isEllipsis" :content="value" />
    </el-space>
  </el-tooltip>
</template>

<script setup lang="ts">
  import { computed } from 'vue'
  const { value, maxLength, prefixLenght, suffixLenght } = defineProps({
    value: {
      type: String
    },
    prefixLenght: {
      type: Number,
      default: 4
    },
    suffixLenght: {
      type: Number,
      default: 4
    },
    maxLength: {
      type: Number,
      default: 10
    },
    copy: {
      type: Boolean,
      default: true
    }
  })
  const isEllipsis = computed(() => {
    return value !== undefined && value?.length > maxLength
  })
  const display = computed(() => {
    if (value == null || value == undefined) return '<Null>'
    if (value == '') return '<Empty>'
    if (value.length <= maxLength) return value
    return `${value.substring(0, prefixLenght)}....${value.substring(value.length - suffixLenght)}`
  })
  const textType = computed(() => {
    return value ? '' : 'info'
  })
</script>

<style scoped lang="scss"></style>
