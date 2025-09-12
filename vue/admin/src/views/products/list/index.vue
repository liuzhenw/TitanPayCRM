<template>
  <FullScreen>
    <div id="container">
      <el-card shadow="never" class="art-table-card">
        <ArtTable row-key="id" :data="tableData" :loading="dataLoading">
          <el-table-column key="id" prop="id" label="标识" width="120" show-overflow-tooltip />
          <el-table-column key="name" prop="name" label="名称" />
          <el-table-column key="price" prop="price" label="单价" align="right" width="100" />
          <el-table-column
            key="salesVolume"
            prop="salesVolume"
            label="销售量"
            align="right"
            width="100"
          />
          <el-table-column key="salesRevenue" prop="salesRevenue" label="销售额" align="right">
            <template #default="{ row }"> {{ row.salesRevenue }} USD </template>
          </el-table-column>
          <el-table-column
            key="totalCommission"
            prop="totalCommission"
            label="总佣金"
            align="right"
          >
            <template #default="{ row }"> {{ row.totalCommission }} USD </template>
          </el-table-column>
          <el-table-column key="createdAt" prop="createdAt" label="创建时间" align="right">
            <template #default="{ row }">
              <Datetime :value="row.createdAt" />
            </template>
          </el-table-column>
          <el-table-column key="actions" align="right" width="140">
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
  import { ProductService, ProductDto } from '@/api/services'
  import { ref } from 'vue'

  const tableData = ref<ProductDto[]>([])
  const dataLoading = ref(false)
  const fetchData = async () => {
    dataLoading.value = true
    tableData.value = await ProductService.getList().finally(() => (dataLoading.value = false))
  }

  onMounted(() => {
    fetchData()
  })
</script>

<style scoped lang="scss"></style>
