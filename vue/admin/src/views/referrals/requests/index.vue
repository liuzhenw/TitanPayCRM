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
        <ArtTableHeader @refresh="onSearch"></ArtTableHeader>
        <ArtTable
          row-key="id"
          :data="tableData"
          :loading="dataLoading"
          v-model:pagination="pagination"
          :marginTop="10"
          @pagination:current-change="onPaginationChange"
          @pagination:size-change="onPaginationChange"
          @sort-change="onSortChange"
        >
          <el-table-column key="email" prop="user.email" label="邮箱地址" show-overflow-tooltip>
            <template #default="{ row }">
              {{ row.user.email }}
            </template>
          </el-table-column>
          <el-table-column key="level" prop="level.name" label="申请等级">
            <template #default="{ row }">
              <LevelTag :value="row.level" />
            </template>
          </el-table-column>
          <el-table-column key="status" prop="status" label="状态" width="100">
            <template #default="{ row }">
              <StatusTag :value="row.status" />
            </template>
          </el-table-column>
          <el-table-column key="createdAt" prop="createdAt" label="申请时间" align="right">
            <template #default="{ row }">
              <Datetime :value="row.createdAt" />
            </template>
          </el-table-column>
          <el-table-column key="actions" width="140" align="right">
            <template #default="{ row }">
              <el-space>
                <ArtButtonTable type="edit" />
                <ArtButtonTable type="delete" />
              </el-space>
            </template>
          </el-table-column>
        </ArtTable>
      </el-card>
    </div>
  </FullScreen>
</template>

<script setup lang="ts">
  import { ref } from 'vue'
  import {
    ReferrerRequestService,
    ReferrerRequestDto,
    ReferrerRequestQueryInput,
    ReferralLevelService
  } from '@/api/services'
  import { SearchFormItem } from '@/types'
  import LevelTag from '../levelTag.vue'
  import StatusTag from './statusTag.vue'

  const tableData = ref<ReferrerRequestDto[]>([])
  const filter = reactive<ReferrerRequestQueryInput>({})
  const filterItems: SearchFormItem[] = [
    {
      label: '邮箱地址',
      prop: 'id',
      type: 'userSearch',
      config: {
        clearable: true
      }
    },
    {
      label: '申请状态',
      prop: 'status',
      type: 'select',
      options: ReferrerRequestService.getStatusOptions(),
      config: {
        clearable: true
      }
    },
    {
      label: '申请等级',
      prop: 'levelId',
      type: 'select',
      options: ()=> ReferralLevelService.getOptions(),
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
  const fetchData = async (input: ReferrerRequestQueryInput) => {
    dataLoading.value = true
    try {
      const res = await ReferrerRequestService.getList(input)
      pagination.total = res.totalCount
      tableData.value = res.items
    } finally {
      dataLoading.value = false
    }
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
    filter.levelId = undefined
    filter.status = undefined
    onSearch()
  }

  onMounted(() => {
    fetchData(filter)
  })
</script>

<style scoped lang="scss"></style>
