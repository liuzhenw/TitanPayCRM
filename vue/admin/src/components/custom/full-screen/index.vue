<template>
  <div class="full-screen" :style="containerStyle">
    <slot></slot>
  </div>
</template>

<script lang="ts" setup>
  import { useCommon } from '@/composables/useCommon'
  const { occupyHeight } = defineProps({
    occupyHeight: {
      type: Number,
      default: 0
    }
  })
  const { workTabHeight } = useCommon()
  const containerStyle = computed(() => {
    return {
      height: `calc(100vh - ${workTabHeight.value + occupyHeight}px)`
    }
  })
</script>

<style lang="scss" scoped>
  .full-screen {
    :deep(#container) {
      display: flex;
      flex-direction: column;
      height: 100%;

      @media (max-width: $device-phone) {
        height: auto;
      }
    }
  }
</style>
