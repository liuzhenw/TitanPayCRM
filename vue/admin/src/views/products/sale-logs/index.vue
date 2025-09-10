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
          @pagination:current-change="onPaginationCurrentChange"
          @pagination:size-change="onPaginationSizeChange"
          @sort-change="onSortChange"
        >
          <el-table-column key="product" prop="product" label="商品名称" show-overflow-tooltip>
            <template #default="{ row }">
              <div>{{ row.product.name }}</div>
            </template>
          </el-table-column>
          <el-table-column key="customer" prop="customer" label="买家" show-overflow-tooltip>
            <template #default="{ row }">
              <div>{{ row.customer.email }}</div>
            </template>
          </el-table-column>
          <el-table-column key="orderNo" prop="orderNo" label="订单号" show-overflow-tooltip />
          <el-table-column key="quantity" prop="quantity" label="数量" width="80" align="right" />
          <el-table-column key="amount" prop="amount" label="金额" width="120" align="right">
            <template #default="{ row }"> {{ row.amount }} USD </template>
          </el-table-column>
           <el-table-column key="amount" prop="amount" label="佣金" width="120" align="right">
            <template #default="{ row }"> {{ row.totalCommission }} USD </template>
          </el-table-column>
          <el-table-column key="createdAt" prop="createdAt" label="创建时间" align="right" show-overflow-tooltip>
            <template #default="{ row }">
              <Datetime :value="row.createdAt" />
            </template>
          </el-table-column>
          <el-table-column key="actions" width="110" align="right">
            <template #default="{ row }">
              <el-space>
                <ArtButtonTable type="edit" />
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
  import {
    ProductSaleLogService,
    ProductSaleLogDto,
    ProductSaleLogQueryInput,
    ProductService
  } from '@/api/services'
  import { SearchFormItem } from '@/types'

  const tableData = ref<ProductSaleLogDto[]>([])
  const filter = reactive<ProductSaleLogQueryInput>({})
  const filterItems: SearchFormItem[] = [
    {
      label: '商品',
      prop: 'productId',
      type: 'select',
      options: () => ProductService.getOptions(),
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
  const fetchData = async (input: ProductSaleLogQueryInput) => {
    dataLoading.value = true
    try {
      const res = await ProductSaleLogService.getList(input)
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
    onSearch()
  }

  onMounted(() => {
    fetchData(filter)
  })
</script>

<style scoped lang="scss"></style>
