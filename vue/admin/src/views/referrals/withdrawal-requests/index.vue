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
            <span style="font-weight: 500; color: #666">提现申请列表</span>
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
          <el-table-column
            key="referrer"
            prop="referrer.email"
            label="申请人"
            show-overflow-tooltip
          >
            <template #default="{ row }">
              {{ row.referrer.email }}
            </template>
          </el-table-column>
          <el-table-column
            key="amount"
            prop="amount"
            label="提现金额"
            align="right"
            width="120"
            sortable="custom"
          >
            <template #default="{ row }"> {{ row.amount }} USDT </template>
          </el-table-column>
          <el-table-column key="toAddress" prop="toAddress" label="提现地址" show-overflow-tooltip>
            <template #default="{ row }">
              <EllipticalLabel :value="row.toAddress" />
            </template>
          </el-table-column>
          <el-table-column key="txid" prop="txid" label="交易哈希" show-overflow-tooltip>
            <template #default="{ row }">
              <EllipticalLabel :value="row.txid" />
            </template>
          </el-table-column>
          <el-table-column key="status" prop="status" label="状态" width="100" align="center">
            <template #default="{ row }">
              <StatusTag :value="row.status" />
            </template>
          </el-table-column>
          <el-table-column
            key="createdAt"
            prop="createdAt"
            label="申请时间"
            align="right"
            width="160"
            sortable="custom"
            show-overflow-tooltip
          >
            <template #default="{ row }">
              <Datetime :value="row.createdAt" />
            </template>
          </el-table-column>
          <el-table-column key="actions" width="80" align="right">
            <template #default="{ row }">
              <el-space>
                <ArtButtonTable type="view" @click="onView(row)" />
              </el-space>
            </template>
          </el-table-column>
        </ArtTable>
      </el-card>
    </div>
  </FullScreen>
</template>

<script setup lang="ts">
  import { ref, reactive } from 'vue'
  import { useRouter } from 'vue-router'
  import {
    WithdrawalRequestService,
    WithdrawalRequestDto,
    WithdrawalRequestQueryInput
  } from '@/api/services'
  import { SearchFormItem } from '@/types'
  import StatusTag from './statusTag.vue'

  const router = useRouter()
  const tableData = ref<WithdrawalRequestDto[]>([])
  const filter = reactive<WithdrawalRequestQueryInput>({})
  const filterItems: SearchFormItem[] = [
    {
      label: '申请用户',
      prop: 'referrerId',
      type: 'userSearch',
      config: {
        clearable: true
      }
    },
    {
      label: '状态',
      prop: 'status',
      type: 'select',
      options: [
        { label: '待处理', value: 'pending' },
        { label: '已批准', value: 'approved' },
        { label: '已拒绝', value: 'rejected' }
      ],
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

  const fetchData = async (input: WithdrawalRequestQueryInput) => {
    dataLoading.value = true
    try {
      const res = await WithdrawalRequestService.getList(input)
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
    filter.referrerId = undefined
    filter.status = undefined
    filter.toAddress = undefined
    filter.txid = undefined
    onSearch()
  }

  const onView = (row: WithdrawalRequestDto) => {
    router.push(`/referrals/withdrawal-requests/${row.id}`)
  }

  // Initialize
  onMounted(() => {
    fetchData(filter)
  })
</script>

<style scoped lang="scss">
  .art-table-card {
    margin-top: 16px;
  }
</style>