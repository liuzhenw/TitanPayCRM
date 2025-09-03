<template>
  <el-popover placement="top">
    <div style="display: flex; justify-content: center">
      <QrcodeVue :value="value" :config="qrcodeConfig" />
    </div>
    <template #reference>
      <i class="iconfont-sys iconsys-erweima2" style="cursor: pointer"></i>
    </template>
  </el-popover>
</template>

<script setup lang="ts">
  import QrcodeVue, { Level, RenderAs } from 'qrcode.vue'
  const { size, value } = defineProps({
    value: { type: String, required: true },
    size: { type: Number, default: 120 },
  })
  const qrcodeConfig = {
    size: size,
    level: 'H' as Level,
    renderAs: 'canvas' as RenderAs,
    margin: 10,
    background: '#ffffff',
    foreground: '#000000'
  }
</script>

<style scoped lang="scss"></style>
