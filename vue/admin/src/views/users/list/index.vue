<template>
  <FullScreen>
    <div id="container">
      <ArtSearchBar
        v-model:filter="filter"
        :items="filterItems"
        @search="onSearch"
        @reset="onReset"
      />
      <el-card shadow="never" class="art-table-card">
        <ArtTable
          row-key="id"
          :data="tableData"
          :loading="dataLoading"
          :marginTop="10"
          :pagination="pagination"
          @pagination:current-change="onPaginationCurrentChange"
          @pagination:size-change="onPaginationSizeChange"
          @sort-change="onSortChange"
        >
          <el-table-column key="name" prop="name" label="名称">
            <template #default="{ row }">
              {{ row.name }}
            </template>
          </el-table-column>
          <el-table-column key="email" prop="email" label="邮箱" show-overflow-tooltip>
            <template #default="{ row }">
              {{ row.email }}
            </template>
          </el-table-column>
          <el-table-column
            key="consumptionCount"
            prop="consumptionCount"
            label="消费次数"
            align="right"
            sortable="custom"
          >
            <template #default="{ row }">
              {{ row.consumptionCount }}
            </template>
          </el-table-column>
          <el-table-column
            key="totalConsumption"
            prop="totalConsumption"
            label="累计消费"
            width="120"
            align="right"
            sortable="custom"
          >
            <template #default="{ row }"> {{ row.totalConsumption }} USD </template>
          </el-table-column>
          <el-table-column
            key="createdAt"
            width="180"
            label="创建时间"
            align="right"
            sortable="custom"
          >
            <template #default="{ row }">
              <Datetime :value="row.createdAt" />
            </template>
          </el-table-column>
          <el-table-column key="actions" width="100" align="right">
            <template #default="{ row }">
              <ArtButtonTable type="view" @click="toDetailsPage(row)" />
            </template>
          </el-table-column>
        </ArtTable>
      </el-card>
    </div>
  </FullScreen>
</template>

<script setup lang="ts">
  import { ref } from 'vue'
  import { UserService, UserDto, UserQueryInput } from '@/api/services'
  import { SearchFormItem } from '@/types'
  import { RoutesAlias } from '@/router/routesAlias'

  const router = useRouter()
  const tableData = ref<UserDto[]>([])
  const filter = reactive<UserQueryInput>({})
  const filterItems: SearchFormItem[] = [
    {
      label: '邮箱地址',
      prop: 'email',
      type: 'input',
      config: {
        clearable: true
      }
    }
  ]
  const pagination = reactive({
    current: 1,
    size: 10,
    total: 0
  })

  const dataLoading = ref(false)
  const fetchData = async (input: UserQueryInput) => {
    dataLoading.value = true
    try {
      const res = await UserService.getList(input)
      pagination.total = res.totalCount
      tableData.value = res.items
    } finally {
      dataLoading.value = false
    }
  }
  const onPaginationCurrentChange = (val: number) => {
    pagination.current = val
    onPaginationChange()
  }
  const onPaginationSizeChange = (val: number) => {
    pagination.size = val
    pagination.current = 1
    onPaginationChange()
  }
  const onPaginationChange = () => {
    filter.maxResultCount = pagination.size
    filter.skipCount = (pagination.current - 1) * pagination.size
    fetchData(filter)
  }
  const onSortChange = (data: { column: any; prop: string; order: any }) => {
    if (!data.prop || !data.order) {
      filter.sorting = undefined
    } else {
      const field = data.prop[0].toUpperCase() + data.prop.slice(1)
      const dire = data.order === 'ascending' ? 'asc' : 'desc'
      filter.sorting = `${field} ${dire}`
    }
    pagination.current = 1
    fetchData(filter)
  }
  const onSearch = () => {
    pagination.current = 1
    fetchData(filter)
  }
  const onReset = () => {
    filter.name = undefined
    onSearch()
  }
  const toDetailsPage = (item: UserDto) => {
    router.push(RoutesAlias.UserDetails.replace(':id', item.id))
  }

  onMounted(() => {
    fetchData(filter)
  })
</script>

<style scoped lang="scss"></style>
