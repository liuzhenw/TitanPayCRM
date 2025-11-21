<template>
  <el-dialog v-model="visible" title="添加直推关系" width="500px">
    <el-form ref="formRef" :model="model" :rules="rules" label-width="auto" label-suffix=":">
      <el-form-item label="被推荐人" prop="recommendeeId">
        {{ user.name }}
      </el-form-item>
      <el-form-item label="直推用户" prop="recommenderId">
        <UserSearchInput v-model:value="model.recommenderId" />
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="visible = false">取消</el-button>
      <el-button type="primary" :loading="loading" @click="onSubmit">确定</el-button>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
  import { ReferralRelationCreateInput, ReferralRelationService, UserDto } from '@/api/services'
  import { ElMessage, FormInstance, FormRules } from 'element-plus'

  const { user } = defineProps<{
    user: UserDto
  }>()
  const visible = defineModel<boolean>()
  const loading = defineModel<boolean>('loading')
  const emit = defineEmits<{
    (e: 'close'): void
  }>()
  const formRef = ref<FormInstance>()
  const model = ref<ReferralRelationCreateInput>({
    recommendeeId: user.id,
    recommenderId: ''
  })
  watch(
    () => user,
    () => {
      model.value.recommendeeId = user.id
    }
  )

  const rules: FormRules = {
    recommenderId: [{ required: true, message: '请选择推荐人', trigger: 'change' }]
  }

  const onSubmit = async () => {
    const valid = await formRef.value?.validate()
    if (!valid) return

    loading.value = true
    await ReferralRelationService.create(model.value).finally(() => (loading.value = false))
    emit('close')
    visible.value = false
    ElMessage.success('添加成功')
  }
</script>

<style scoped lang="scss"></style>
