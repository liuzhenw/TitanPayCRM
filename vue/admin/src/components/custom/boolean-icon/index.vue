<template>
  <i class="iconfont-sys" :style="{ color: colorStyle }">
    {{ value ? '&#xe838;' : '&#xe83a;' }}
  </i>
</template>

<script setup lang="ts">
  const { value } = defineProps<{ value: boolean }>()
  const colorStyle = computed(() => {
    return value ? '#67C23A' : '#F56C6C'
  })
</script>

<style scoped lang="scss"></style>
