<template>
  <div class="commission-logs">
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
        <el-table-column key="product" prop="product.name" label="产品名称" show-overflow-tooltip>
          <template #default="{ row }">
            {{ row.product.name }}
          </template>
        </el-table-column>
        <el-table-column key="receiver" prop="receiver.email" label="推荐人" show-overflow-tooltip>
          <template #default="{ row }">
            {{ row.receiver.email }}
          </template>
        </el-table-column>
        <el-table-column
          key="customer"
          prop="customer.email"
          label="被推荐人"
          show-overflow-tooltip
        >
          <template #default="{ row }">
            {{ row.customer.email }}
          </template>
        </el-table-column>
        <el-table-column key="level" prop="level.name" label="推荐等级" sortable="custom">
          <template #default="{ row }">
            <LevelTag :value="row.level" />
          </template>
        </el-table-column>
        <el-table-column
          key="referralDepth"
          prop="referralDepth"
          label="推荐层级"
          width="110"
          align="right"
          sortable="custom"
        >
          <template #default="{ row }">
            {{ row.referralDepth }}
          </template>
        </el-table-column>
        <el-table-column
          key="amount"
          prop="amount"
          label="佣金金额"
          width="120"
          align="right"
          sortable="custom"
        >
          <template #default="{ row }">
            {{ formatAmount(row.amount) }}
          </template>
        </el-table-column>
        <el-table-column
          key="createdAt"
          prop="createdAt"
          label="创建时间"
          align="right"
          sortable="custom"
        >
          <template #default="{ row }">
            <Datetime :value="row.createdAt" />
          </template>
        </el-table-column>
        <el-table-column key="actions" width="80" align="right">
          <template #default>
            <el-space>
              <ArtButtonTable type="view" />
            </el-space>
          </template>
        </el-table-column>
      </ArtTable>
    </el-card>
  </div>
</template>

<script setup lang="ts">
  import { ref, reactive, onMounted, watch } from 'vue'
  import { CommissionLogService, CommissionLogDto, CommissionLogQueryInput } from '@/api/services'
  import LevelTag from '../../levelTag.vue'

  interface Props {
    referrerId: string
  }

  const props = defineProps<Props>()

  const tableData = ref<CommissionLogDto[]>([])
  const filter = reactive<CommissionLogQueryInput>({
    receiverId: props.referrerId
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
    } catch (error) {
      console.error('获取佣金记录失败:', error)
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

  watch(
    () => props.referrerId,
    (newId) => {
      if (newId) {
        filter.receiverId = newId
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
  .commission-logs {
    .art-table-card {
      border-radius: 8px;
      overflow: hidden;
    }
  }
</style>
