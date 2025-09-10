<template>
  <FullScreen :occupy-height="85">
    <div id="container">
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
          key="receiver"
          prop="receiver.email"
          label="上级用户"
          show-overflow-tooltip
        >
          <template #default="{ row }">
            {{ row.receiver.email }}
          </template>
        </el-table-column>
        <el-table-column key="level" prop="level.name" label="推荐等级" width="100">
          <template #default="{ row }">
            <LevelTag :value="row.level" />
          </template>
        </el-table-column>
        <el-table-column
          key="referralDepth"
          prop="referralDepth"
          label="推荐层级"
          width="80"
          align="right"
        >
          <template #default="{ row }">
            {{ row.referralDepth }}
          </template>
        </el-table-column>
        <el-table-column key="amount" prop="amount" label="佣金金额" width="120" align="right">
          <template #default="{ row }">
            {{ formatAmount(row.amount) }}
          </template>
        </el-table-column>
        <el-table-column
          key="createdAt"
          prop="createdAt"
          label="创建时间"
          align="right"
          show-overflow-tooltip
        >
          <template #default="{ row }">
            <Datetime :value="row.createdAt" />
          </template>
        </el-table-column>
        <el-table-column key="actions" width="80" align="right">
          <template #default="{ row }">
            <el-space>
              <ArtButtonTable type="view" />
            </el-space>
          </template>
        </el-table-column>
      </ArtTable>
    </div>
  </FullScreen>
</template>

<script setup lang="ts">
  import { ref, reactive, onMounted } from 'vue'
  import {
    CommissionLogService,
    CommissionLogDto,
    CommissionLogQueryInput,
    ProductSaleLogDto
  } from '@/api/services'
  import LevelTag from '@/views/referrals/levelTag.vue'

  const props = defineProps<{
    saleLog: ProductSaleLogDto
  }>()

  const tableData = ref<CommissionLogDto[]>([])
  const filter = reactive<CommissionLogQueryInput>({
    saleLogId: props.saleLog.id
  })

  const pagination = reactive({
    current: 1,
    size: 10,
    total: 0
  })
  const dataLoading = ref(false)

  const formatAmount = (amount: number) => {
    return amount.toFixed(2)
  }

  const fetchData = async (input: CommissionLogQueryInput) => {
    dataLoading.value = true
    try {
      const res = await CommissionLogService.getList(input)
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
    filter.saleLogId = props.saleLog.id
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

  const refreshData = () => {
    pagination.current = 1
    fetchData(filter)
  }

  onMounted(() => {
    fetchData(filter)
  })
</script>

<style scoped lang="scss">
  .commission-logs {
    .card-header {
      display: flex;
      justify-content: space-between;
      align-items: center;

      .header-actions {
        display: flex;
        gap: 8px;
      }
    }

    .empty-state {
      margin-top: 40px;
    }
  }
</style>
