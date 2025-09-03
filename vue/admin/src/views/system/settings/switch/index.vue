<template>
  <el-switch v-model="value" />
</template>

<script setup lang="ts">
  defineOptions({ name: 'SettingSwitch' })
  const model = defineModel<string | null>()

  const value = ref<boolean>(false)
  watchEffect(() => {
    value.value = model.value === 'true'
  })
  watch(value, (newValue) => {
    model.value = newValue ? 'true' : 'false'
  })
</script>

<style scoped lang="scss"></style>
