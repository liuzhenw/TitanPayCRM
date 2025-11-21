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
    <el-option v-for="user in users" :key="user.id" :value="user.id" :label="user.name" />
  </el-select>
</template>

<script setup lang="ts">
  import { UserService, UserDto } from '@/api/services'
  import { SearchFormItem } from '@/types'

  const value = defineModel<string>('value')
  const { item } = defineProps({
    item: {
      type: Object as () => SearchFormItem
    }
  })
  const config = reactive({
    placeholder: `请输入用户名`,
    ...(item?.config || {})
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
  const onSelected = (userId: string | undefined) => {
    console.debug('onSelected', userId)
    if (!userId) {
      value.value = undefined
      return
    }

    value.value = userId
  }

  onMounted(async () => {
    if (value.value) {
      const res = await UserService.get(value.value as string)
      searchValue.value = res.name
    }
  })
</script>

<style scoped lang="scss"></style>
