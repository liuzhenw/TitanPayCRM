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
        <ArtTableHeader @refresh="onSearch">
          <template #left>
            <el-button icon="Plus" @click="onCreate">创建</el-button>
          </template>
        </ArtTableHeader>
        <ArtTable
          row-key="id"
          :data="tableData"
          :loading="dataLoading"
          v-model:pagination="pagination"
          :marginTop="10"
          @pagination:current-change="onPaginationCurrentChange"
          @pagination:size-change="onPaginationSizeChange"
          @sort-change="onSortChange"
        >
          <el-table-column key="user" prop="user.name" label="邮箱地址" show-overflow-tooltip>
            <template #default="{ row }">
              {{ row.user.email }}
            </template>
          </el-table-column>
          <el-table-column key="level" prop="level.name" label="推荐等级" width="100">
            <template #default="{ row }">
              <LevelTag :value="row.level" />
            </template>
          </el-table-column>
          <el-table-column
            key="directCount"
            prop="directCount"
            label="直推人数"
            align="right"
            width="110"
            sortable="custom"
          >
            <template #default="{ row }">
              {{ row.directCount }}
            </template>
          </el-table-column>
          <el-table-column
            key="indirectCount"
            prop="indirectCount"
            label="间推人数"
            align="right"
            width="110"
            sortable="custom"
          >
            <template #default="{ row }">
              {{ row.indirectCount }}
            </template>
          </el-table-column>
          <el-table-column
            key="commission"
            prop="commission"
            label="可用佣金"
            align="right"
            width="110"
            sortable="custom"
            show-overflow-tooltip
          >
            <template #default="{ row }">
              {{ row.commission }} USD
            </template>
          </el-table-column>
          <el-table-column
            key="totalCommission"
            prop="totalCommission"
            label="累计佣金"
            align="right"
            width="110"
            sortable="custom"
            show-overflow-tooltip
          >
            <template #default="{ row }">
              {{ row.totalCommission }} USD
            </template>
          </el-table-column>
          <el-table-column key="status" prop="isDisabled" label="状态" width="80" align="center">
            <template #default="{ row }">
              <BooleanIcon :value="!row.isDisabled" />
            </template>
          </el-table-column>
          <el-table-column
            key="createdAt"
            width="160"
            label="创建时间"
            align="right"
            sortable="custom"
            show-overflow-tooltip
          >
            <template #default="{ row }">
              <Datetime :value="row.createdAt" />
            </template>
          </el-table-column>

          <el-table-column key="actions" width="110" align="right">
            <template #default="{ row }">
              <el-space>
                <ArtButtonTable type="edit" @click="onEdit(row)" />
              </el-space>
            </template>
          </el-table-column>
        </ArtTable>
      </el-card>
    </div>
    <ReferrerCreateDialog v-model="createVisible" @close="onSearch" />
    <ReferrerUpdateDrawer
      v-if="selectedReferrer"
      v-model="updateVisible"
      :value="selectedReferrer"
      @close="onSearch"
    />
  </FullScreen>
</template>

<script setup lang="ts">
  import { ref } from 'vue'
  import { ReferrerService, ReferrerDto, ReferrerQueryInput } from '@/api/services'
  import { SearchFormItem } from '@/types'
  import LevelTag from '../levelTag.vue'
  import ReferrerCreateDialog from './create.vue'
  import ReferrerUpdateDrawer from './update.vue'
  import { on } from 'events'

  const tableData = ref<ReferrerDto[]>([])
  const filter = reactive<ReferrerQueryInput>({})
  const filterItems: SearchFormItem[] = [
    {
      label: '邮箱地址',
      prop: 'id',
      type: 'userSearch',
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
  const fetchData = async (input: ReferrerQueryInput) => {
    dataLoading.value = true
    try {
      const res = await ReferrerService.getList(input)
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
    fetchData(filter)
  }
  const onSearch = () => {
    pagination.current = 1
    fetchData(filter)
  }
  const onReset = () => {
    filter.id = undefined
    onSearch()
  }

  const createVisible = ref(false)
  const onCreate = () => {
    createVisible.value = true
  }

  const updateVisible = ref(false)
  const selectedReferrer = ref<ReferrerDto>()
  const onEdit = (row: ReferrerDto) => {
    selectedReferrer.value = row
    updateVisible.value = true
  }

  onMounted(() => {
    fetchData(filter)
  })
</script>

<style scoped lang="scss"></style>
