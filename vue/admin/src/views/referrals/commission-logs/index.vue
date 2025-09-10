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
          <el-table-column key="customer" prop="customer.email" label="被推荐人" show-overflow-tooltip>
            <template #default="{ row }">
              {{ row.customer.email }}
            </template>
          </el-table-column>
          <el-table-column key="level" prop="level.name" label="推荐等级" sortable="custom">
            <template #default="{ row }">
              <LevelTag :value="row.level" />
            </template>
          </el-table-column>
          <el-table-column key="referralDepth" prop="referralDepth" label="推荐层级" width="110" align="right" sortable="custom">
            <template #default="{ row }">
              {{ row.referralDepth }}
            </template>
          </el-table-column>
          <el-table-column key="amount" prop="amount" label="佣金金额" width="120" align="right" sortable="custom">
            <template #default="{ row }">
              {{ formatAmount(row.amount) }}
            </template>
          </el-table-column>
          <el-table-column key="createdAt" prop="createdAt" label="创建时间" align="right" sortable="custom">
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
      </el-card>
    </div>
  </FullScreen>
</template>

<script setup lang="ts">
  import { ref, reactive, onMounted } from 'vue'
  import {
    CommissionLogService,
    CommissionLogDto,
    CommissionLogQueryInput,
    ProductService,
    ReferralLevelService
  } from '@/api/services'
  import { SearchFormItem } from '@/types'
  import LevelTag from '../levelTag.vue'

  const tableData = ref<CommissionLogDto[]>([])
  const filter = reactive<CommissionLogQueryInput>({})
  const filterItems: SearchFormItem[] = [
    {
      label: '产品',
      prop: 'productId',
      type: 'select',
      options: async () => {
        const products = await ProductService.getOptions()
        return products
      },
      config: {
        clearable: true
      }
    },
    {
      label: '接收人',
      prop: 'receiverId',
      type: 'userSearch',
      config: {
        clearable: true
      }
    },
    {
      label: '客户',
      prop: 'customerId',
      type: 'userSearch',
      config: {
        clearable: true
      }
    },
    {
      label: '推荐等级',
      prop: 'levelId',
      type: 'select',
      options: async () => {
        const levels = await ReferralLevelService.getOptions()
        return levels
      },
      config: {
        clearable: true
      }
    },
    {
      label: '推荐层级',
      prop: 'referralDepth',
      type: 'input',
      config: {
        clearable: true,
        placeholder: '请输入推荐层级'
      }
    }
  ]

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
    filter.productId = undefined
    filter.receiverId = undefined
    filter.customerId = undefined
    filter.levelId = undefined
    filter.referralDepth = undefined
    onSearch()
  }

  onMounted(() => {
    fetchData(filter)
  })
</script>

<style scoped lang="scss"></style>