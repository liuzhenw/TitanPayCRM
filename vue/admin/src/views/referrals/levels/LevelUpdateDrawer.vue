<template>
  <el-drawer v-model="visible" :title="title" :before-close="handleClose" destroy-on-close>
    <el-form ref="formRef" :model="form" :rules="rules" label-width="100px" label-position="left">
      <el-form-item label="等级标识" prop="id">
        <el-input
          required
          v-model="levelId"
          placeholder="请输入等级标识"
          maxlength="32"
          show-word-limit
          :disabled="props.level != null"
        />
      </el-form-item>

      <el-form-item label="等级名称" prop="name">
        <el-input v-model="form.name" placeholder="请输入等级名称" maxlength="32" show-word-limit />
      </el-form-item>

      <el-form-item label="等级大小" prop="size">
        <el-input-number
          v-model="form.size"
          :min="0"
          :max="999999"
          placeholder="请输入等级大小"
          style="width: 100%"
        />
      </el-form-item>

      <el-form-item label="佣金系数" prop="multiplier">
        <el-input-number
          v-model="form.multiplier"
          :min="0"
          :max="10"
          :step="0.1"
          :precision="2"
          placeholder="请输入佣金系数"
          style="width: 100%"
        >
          <template #suffix>
            <span style="margin-right: 8px">倍</span>
          </template>
        </el-input-number>
      </el-form-item>

      <el-form-item label="描述" prop="description">
        <el-input
          v-model="form.description"
          type="textarea"
          :rows="3"
          placeholder="请输入等级描述（选填）"
          maxlength="200"
          show-word-limit
        />
      </el-form-item>
    </el-form>

    <template #footer>
      <div class="drawer-footer">
        <el-button @click="handleClose">取消</el-button>
        <el-button type="primary" :loading="loading" @click="handleSubmit"> 保存 </el-button>
      </div>
    </template>
  </el-drawer>
</template>

<script setup lang="ts">
  import { ref, reactive, computed, watch } from 'vue'
  import { ElMessage, type FormInstance, type FormRules } from 'element-plus'
  import { ReferralLevelService, ReferralLevelDto, ReferralLevelUpdateInput } from '@/api/services'

  interface Props {
    modelValue: boolean
    level?: ReferralLevelDto
  }

  interface Emits {
    (e: 'update:modelValue', value: boolean): void
    (e: 'success'): void
  }

  const props = defineProps<Props>()
  const emit = defineEmits<Emits>()

  const formRef = ref<FormInstance>()
  const loading = ref(false)

  const visible = computed({
    get: () => props.modelValue,
    set: (value) => emit('update:modelValue', value)
  })

  const title = computed(() => (props.level ? '编辑推荐等级' : '创建推荐等级'))

  const levelId = ref('')
  const form = reactive<ReferralLevelUpdateInput>({
    name: '',
    size: 1,
    multiplier: 0,
    description: ''
  })

  const rules: FormRules = {
    // id: [
    //   { required: true, message: '请输入等级标识', trigger: 'blur' },
    //   { min: 1, max: 32, message: '等级名称长度在 1 到 32 个字符', trigger: 'blur' }
    // ],
    name: [
      { required: true, message: '请输入等级名称', trigger: 'blur' },
      { min: 1, max: 32, message: '等级名称长度在 1 到 32 个字符', trigger: 'blur' }
    ],
    size: [
      { required: true, message: '请输入等级大小', trigger: 'blur' },
      {
        type: 'number',
        min: 1,
        max: 999999,
        message: '等级大小必须在 1 到 999999 之间',
        trigger: 'blur'
      }
    ],
    multiplier: [
      { required: true, message: '请输入佣金系数', trigger: 'blur' },
      { type: 'number', min: 0, max: 10, message: '佣金系数必须在 0 到 10 之间', trigger: 'blur' }
    ]
  }

  const resetForm = () => {
    if (formRef.value) {
      formRef.value.resetFields()
    }

    if (props.level) {
      form.name = props.level.name
      form.size = props.level.size
      form.multiplier = props.level.multiplier
      form.description = props.level.description
    } else {
      levelId.value = ''
      form.name = ''
      form.size = 1
      form.multiplier = 0
      form.description = ''
    }
  }

  const handleClose = () => {
    visible.value = false
  }

  const handleSubmit = async () => {
    if (!formRef.value) return

    try {
      await formRef.value.validate()
      loading.value = true

      if (props.level) {
        // Update existing level
        await ReferralLevelService.update(props.level.id, {
          name: form.name,
          size: form.size,
          multiplier: form.multiplier,
          description: form.description
        })
        ElMessage.success('更新成功')
      } else {
        // Create new level
        await ReferralLevelService.create({
          id: levelId.value,
          name: form.name,
          size: form.size,
          multiplier: form.multiplier,
          description: form.description
        })
        ElMessage.success('创建成功')
      }

      emit('success')
      visible.value = false
    } catch (error) {
      console.error('保存推荐等级失败:', error)
      ElMessage.error('保存推荐等级失败')
    } finally {
      loading.value = false
    }
  }

  watch(
    () => props.modelValue,
    (newValue) => {
      if (newValue) {
        resetForm()
      }
    },
    { immediate: true }
  )

  watch(
    () => props.level,
    () => {
      if (props.modelValue) {
        resetForm()
      }
    }
  )
</script>

<style scoped lang="scss">
  .drawer-footer {
    display: flex;
    justify-content: flex-end;
    gap: 12px;
    padding: 16px 0;
  }
</style>
