<template>
  <div class="withdrawal-requests">
    <el-card shadow="never" class="art-table-card">
      <ArtTableHeader @refresh="onSearch"></ArtTableHeader>
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
        <el-table-column key="toAddress" prop="toAddress" label="提款地址" show-overflow-tooltip />
        <el-table-column
          key="amount"
          prop="amount"
          label="提款金额"
          width="120"
          align="right"
          sortable="custom"
        >
          <template #default="{ row }">
            <span class="withdrawal-amount">{{ row.amount }} USDT</span>
          </template>
        </el-table-column>
        <el-table-column key="status" prop="status" label="状态" width="100" sortable="custom">
          <template #default="{ row }">
            <WithdrawalStatusTag :value="row.status"></WithdrawalStatusTag>
          </template>
        </el-table-column>
        <el-table-column
          key="createdAt"
          prop="createdAt"
          label="申请时间"
          width="180"
          align="right"
          sortable="custom"
        >
          <template #default="{ row }">
            <el-tooltip :content="row.createdAt" placement="top">
              <span>{{ formatDate(row.createdAt) }}</span>
            </el-tooltip>
          </template>
        </el-table-column>
        <el-table-column
          key="completedAt"
          prop="completedAt"
          label="完成时间"
          width="180"
          align="right"
          sortable="custom"
        >
          <template #default="{ row }">
            <el-tooltip v-if="row.completedAt" :content="row.completedAt" placement="top">
              <span>{{ formatDate(row.completedAt) }}</span>
            </el-tooltip>
            <span v-else class="text-muted">-</span>
          </template>
        </el-table-column>
        <el-table-column key="actions" width="80" align="right">
          <template #default="{ row }">
            <el-space>
              <ArtButtonTable type="view" @click="handleViewDetails(row)" />
            </el-space>
          </template>
        </el-table-column>
      </ArtTable>
    </el-card>

    <div v-if="tableData.length === 0 && !dataLoading" class="empty-state">
      <el-empty description="暂无提款申请" />
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, reactive, onMounted, watch } from 'vue'
  import { useRouter } from 'vue-router'
  import {
    WithdrawalRequestService,
    WithdrawalRequestDto,
    WithdrawalRequestQueryInput
  } from '@/api/services'
  import WithdrawalStatusTag from '../../withdrawal-requests/statusTag.vue'

  interface Props {
    referrerId: string
  }

  const props = defineProps<Props>()
  const router = useRouter()

  const tableData = ref<WithdrawalRequestDto[]>([])
  const filter = reactive<WithdrawalRequestQueryInput>({
    referrerId: props.referrerId
  })

  const pagination = reactive({
    current: 1,
    size: 10,
    total: 0
  })
  const dataLoading = ref(false)

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleString('zh-CN')
  }

  const fetchData = async (input: WithdrawalRequestQueryInput) => {
    dataLoading.value = true
    try {
      const res = await WithdrawalRequestService.getList(input)
      pagination.total = res.totalCount
      tableData.value = res.items
    } catch (error) {
      console.error('获取提款申请失败:', error)
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

  const handleViewDetails = (row: WithdrawalRequestDto) => {
    router.push(`/referrals/withdrawal-requests/${row.id}`)
  }

  watch(
    () => props.referrerId,
    (newId) => {
      if (newId) {
        filter.referrerId = newId
        onSearch()
      }
    },
    { immediate: true }
  )

  onMounted(() => {
    fetchData(filter)
  })
</script>

<style scoped lang="scss">
  .withdrawal-requests {
    .art-table-card {
      border-radius: 8px;
      overflow: hidden;
    }

    .withdrawal-amount {
      font-weight: 600;
      color: #e6a23c;
    }

    .text-muted {
      color: #909399;
    }

    .empty-state {
      padding: 40px 0;
      text-align: center;
    }
  }
</style>
