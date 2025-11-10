<template>
  <el-drawer v-model="visible" :title="title" :before-close="handleClose" destroy-on-close>
    <el-form ref="formRef" :model="form" :rules="rules" label-width="100px" label-position="left">
      <el-form-item v-if="!product" label="产品ID" prop="id">
        <el-input
          v-model="form.id"
          placeholder="请输入产品ID（只能包含数字、字母和下划线）"
          maxlength="32"
          show-word-limit
        />
      </el-form-item>

      <el-form-item label="产品名称" prop="name">
        <el-input v-model="form.name" placeholder="请输入产品名称" maxlength="64" show-word-limit />
      </el-form-item>

      <el-form-item label="产品价格" prop="price">
        <el-input-number
          v-model="form.price"
          :min="0"
          :max="999999"
          :precision="2"
          controls-position="right"
          placeholder="请输入产品价格"
          style="width: 100%"
        >
          <template #prefix>
            <span style="margin-left: 8px">$</span>
          </template>
        </el-input-number>
      </el-form-item>

      <el-form-item label="产品描述" prop="description">
        <el-input
          v-model="form.description"
          type="textarea"
          :rows="4"
          placeholder="请输入产品描述（选填）"
          maxlength="1000"
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
  import {
    ProductService,
    ProductDto,
    ProductCreateInput,
    ProductUpdateInput
  } from '@/api/services'
  import { Picture } from '@element-plus/icons-vue'

  interface Props {
    modelValue: boolean
    product?: ProductDto
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

  const title = computed(() => (props.product ? '编辑产品' : '创建产品'))

  const form = reactive<ProductCreateInput>({
    id: '',
    name: '',
    price: 0,
    imageUri: '',
    description: ''
  })

  const rules: FormRules = {
    id: [
      { required: true, message: '请输入产品ID', trigger: 'blur' },
      {
        pattern: /^[a-zA-Z0-9_]+$/,
        message: '产品ID只能包含数字、字母和下划线',
        trigger: 'blur'
      },
      { min: 1, max: 32, message: '产品ID长度在 1 到 32 个字符', trigger: 'blur' }
    ],
    name: [
      { required: true, message: '请输入产品名称', trigger: 'blur' },
      { min: 1, max: 64, message: '产品名称长度在 1 到 64 个字符', trigger: 'blur' }
    ],
    price: [
      { required: true, message: '请输入产品价格', trigger: 'blur' },
      {
        type: 'number',
        min: 0,
        max: 999999,
        message: '产品价格必须在 0 到 999999 之间',
        trigger: 'blur'
      }
    ],
    imageUri: [
      {
        pattern: /^https?:\/\/.+\.(jpg|jpeg|png|gif|webp)$/i,
        message: '请输入有效的图片URL',
        trigger: 'blur'
      }
    ]
  }

  const resetForm = () => {
    if (formRef.value) {
      formRef.value.resetFields()
    }

    if (props.product) {
      form.name = props.product.name
      form.price = props.product.price
      form.imageUri = props.product.imageUri || ''
      form.description = props.product.description || ''
    } else {
      form.id = ''
      form.name = ''
      form.price = 0
      form.imageUri = ''
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

      if (props.product) {
        // Update existing product
        const updateData: ProductUpdateInput = {
          name: form.name,
          price: form.price,
          imageUri: form.imageUri || undefined,
          description: form.description || undefined
        }

        await ProductService.update(props.product.id, updateData)
        ElMessage.success('更新成功')
      } else {
        // Create new product
        const createData: ProductCreateInput = {
          id: form.id,
          name: form.name,
          price: form.price,
          imageUri: form.imageUri || undefined,
          description: form.description || undefined
        }

        await ProductService.create(createData)
        ElMessage.success('创建成功')
      }

      emit('success')
      visible.value = false
    } catch (error) {
      console.error('保存产品失败:', error)
      ElMessage.error('保存产品失败')
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
    () => props.product,
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

  .image-preview {
    margin-top: 8px;
  }

  .image-error {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    height: 100%;
    color: #909399;
    font-size: 12px;

    .el-icon {
      font-size: 24px;
      margin-bottom: 4px;
    }
  }
</style>
