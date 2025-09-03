<template>
  <el-select
    v-bind="config"
    v-model="searchValue"
    filterable
    remote
    :loading="loading"
    :remote-method="onInputChange"
    @change="onSelected"
  >
    <el-option v-for="item in users" :key="item.id" :value="item.id" :label="item.name" />
  </el-select>
</template>

<script setup lang="ts">
  import { UserService, UserDto } from '@/api/services'
  import { SearchFormItem } from '@/types'

  const value = defineModel('value')
  const { item } = defineProps({
    item: {
      type: Object as () => SearchFormItem,
      required: true
    }
  })
  const config = reactive({
    placeholder: `请输入用户名`,
    ...(item.config || {})
  })

  watch(
    () => value.value,
    () => {
      if (!value.value) searchValue.value = ''
    }
  )

  const loading = ref(false)
  const searchValue = ref('')
  const users = ref<UserDto[]>([])
  const onInputChange = async (query: string) => {
    if (!query) {
      users.value = []
      value.value = undefined
      return
    }

    loading.value = true
    const res = await UserService.getList({ name: query }).finally(() => (loading.value = false))
    users.value = res.items
    value.value = users.value[0]?.id
  }
  const onSelected = (item: string | undefined) => {
    console.debug('onSelected', item)
    if (!item) {
      value.value = undefined
      return
    }

    value.value = item
  }

  onMounted(async () => {
    if (value.value) {
      const res = await UserService.get(value.value as string)
      searchValue.value = res.name
    }
  })
</script>

<style scoped lang="scss"></style>
