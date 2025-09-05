<template>
  <el-dialog v-model="modelValue" :close-on-click-modal="false" width="500px" append-to-body @close="$emit('close')">
    <template #title>添加推广员</template>
    <el-form :model="model" ref="form" :rules="rules" label-width="80px">
      <el-form-item label="邮箱地址" prop="email">
        <el-input v-model="model.email" placeholder="请输入用户邮箱" clearable />
      </el-form-item>
      <el-form-item label="推广等级" prop="levelId">
        <el-select v-model="model.levelId" placeholder="请选择推广等级">
          <el-option
            v-for="level in levels"
            :key="level.value"
            :label="level.label"
            :value="level.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="备注说明" prop="remark">
        <el-input
          v-model="model.remark"
          type="textarea"
          :rows="4"
          placeholder="请输入备注信息"
          maxlength="255"
          show-word-limit
        />
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button type="primary" :loading="isSubmitting" @click="onSubmit">提交</el-button>
      <el-button @click="onCancel">取消</el-button>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
  import { ReferralLevelService, ReferrerCreateInput, ReferrerService } from '@/api/services'
  import { ElMessage, FormInstance, FormRules } from 'element-plus'

  defineOptions({
    name: 'ReferrerCreateDialog'
  })
  defineEmits(['close'])
  const modelValue = defineModel({ type: Boolean })
  const levels = ref<{ value: string; label: string }[]>([])

  const model = reactive<ReferrerCreateInput>({
    email: ''
  })
  const rules: FormRules = {
    email: [
      { required: true, message: '请输入邮箱地址', trigger: 'blur' },
      { type: 'email', message: '请输入正确的邮箱地址', trigger: ['blur', 'change'] }
    ],
    levelId: [{ required: true, message: '请选择推广等级', trigger: 'change' }]
  }
  const form = ref<FormInstance>()
  const isSubmitting = ref(false)
  const onSubmit = async () => {
    const valid = await form.value?.validate()
    if (!valid) return

    isSubmitting.value = true
    await ReferrerService.create(model).finally(() => (isSubmitting.value = false))
    ElMessage.success('添加成功')
    modelValue.value = false
  }

  const onCancel = () => {
    model.email = ''
    model.levelId = undefined
    model.remark = undefined
    modelValue.value = false
  }

  onMounted(async () => {
    levels.value = await ReferralLevelService.getOptions()
  })
</script>

<style scoped lang="scss"></style>
