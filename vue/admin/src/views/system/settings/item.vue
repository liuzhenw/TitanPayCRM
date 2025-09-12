<template>
  <el-form-item :key="value.name" :label="value.displayName">
    <div class="setting-item-container">
      <div class="setting-input">
        <slot :item="value">
          <el-input
            v-if="value.type === 'number'"
            type="number"
            v-model="value.value"
            :placeholder="`请输入${value.displayName}`"
            clearable
            style="width: 340px"
          />
          <SettingSwitch v-else-if="value.type === 'boolean'" v-model="value.value" />
          <el-input
            v-else
            v-model="value.value"
            :placeholder="`请输入${value.displayName}`"
            clearable
            style="width: 340px"
          />
        </slot>
      </div>
      <div class="setting-actions">
        <el-tooltip placement="top" :content="value.description">
          <el-icon :size="20" color="#909399"><QuestionFilled /></el-icon>
        </el-tooltip>
        <el-button type="primary" link :disabled="isButtonDisabled" @click="onReset"
          >还原</el-button
        >
        <el-button
          type="primary"
          link
          :loading="loading"
          :disabled="isButtonDisabled"
          @click="onSubmit"
        >
          保存
        </el-button>
      </div>
    </div>
  </el-form-item>
</template>

<script setup lang="ts">
  import { SettingService } from '@/api/services/settings'
  import { SettingItemDto } from '@/api/services/settings/types'
  import { ElMessage } from 'element-plus'
  import SettingSwitch from './switch/index.vue'

  defineOptions({ name: 'SettingItem' })
  const props = defineProps<{
    value: SettingItemDto
  }>()
  const emit = defineEmits<{
    (e: 'updated', value: SettingItemDto): void
  }>()
  const value = ref({ ...props.value })
  watch(
    () => props.value,
    (newValue) => {
      value.value = { ...newValue }
    }
  )

  const hasChanged = computed(() => {
    return value.value.value !== props.value.value
  })

  const isButtonDisabled = computed(() => {
    return !hasChanged.value || loading.value
  })

  const loading = ref(false)
  const onSubmit = async () => {
    loading.value = true
    await SettingService.update({
      name: value.value.name,
      value: value.value.value?.toString()
    }).finally(() => (loading.value = false))
    emit('updated', value.value)
    ElMessage.success('保存成功')
  }
  const onReset = () => {
    value.value.value = props.value.value
  }
</script>

<style scoped lang="scss">
  .setting-item-container {
    display: flex;
    align-items: center;
    gap: 16px;
    width: 100%;
  }

  .setting-input {
    flex: 1;
    min-width: 0;
  }

  .setting-actions {
    display: flex;
    align-items: center;
    gap: 8px;
    flex-shrink: 0;
  }
</style>
