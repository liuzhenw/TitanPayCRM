<template>
  <span>{{ dateTime }}</span>
</template>

<script setup lang="ts">
  import dayjs from 'dayjs'
  const { value, format } = defineProps({
    value: {
      type: String,
      default: ''
    },
    format: {
      type: String,
      default: 'YYYY-MM-DD HH:mm:ss'
    }
  })

  const dateTime = computed(() => {
    if (!value) return '--'
    return dayjs(value).format(format)
  })
</script>

<style scoped lang="scss"></style>
