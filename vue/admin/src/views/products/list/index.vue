<template>
  <FullScreen>
    <div id="container">
      <el-card shadow="never" class="art-table-card">
        <ArtTableHeader @refresh="fetchData">
          <template #left>
            <el-button type="primary" @click="handleCreate">
              <el-icon><Plus /></el-icon>
              新建产品
            </el-button>
          </template>
        </ArtTableHeader>
        <ArtTable row-key="id" :data="tableData" :loading="dataLoading" :marginTop="10">
          <el-table-column key="id" prop="id" label="标识" width="120" show-overflow-tooltip />
          <el-table-column key="name" prop="name" label="名称" />
          <el-table-column key="price" prop="price" label="单价" align="right" width="100">
            <template #default="{ row }"> ${{ row.price }} </template>
          </el-table-column>
          <el-table-column
            key="salesVolume"
            prop="salesVolume"
            label="销售量"
            align="right"
            width="100"
          />
          <el-table-column key="salesRevenue" prop="salesRevenue" label="销售额" align="right">
            <template #default="{ row }"> ${{ formatAmount(row.salesRevenue) }} </template>
          </el-table-column>
          <el-table-column
            key="totalCommission"
            prop="totalCommission"
            label="总佣金"
            align="right"
          >
            <template #default="{ row }"> ${{ formatAmount(row.totalCommission) }} </template>
          </el-table-column>

          <el-table-column key="createdAt" prop="createdAt" label="创建时间" align="right">
            <template #default="{ row }">
              <Datetime :value="row.createdAt" />
            </template>
          </el-table-column>
          <el-table-column key="actions" align="right" width="140">
            <template #default="{ row }">
              <el-space>
                <ArtButtonTable type="edit" @click="handleEdit(row)" />
                <ArtButtonTable type="delete" @click="handleDelete(row)" />
              </el-space>
            </template>
          </el-table-column>
        </ArtTable>
      </el-card>

      <!-- Product Update Drawer -->
      <ProductUpdateDrawer
        v-model="drawerVisible"
        :product="currentProduct"
        @success="handleDrawerSuccess"
      />
    </div>
  </FullScreen>
</template>

<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { ElMessage, ElMessageBox } from 'element-plus'
  import { ProductService, ProductDto } from '@/api/services'
  import ProductUpdateDrawer from './ProductUpdateDrawer.vue'
  import { Plus } from '@element-plus/icons-vue'

  const tableData = ref<ProductDto[]>([])
  const dataLoading = ref(false)
  const drawerVisible = ref(false)
  const currentProduct = ref<ProductDto>()

  const formatAmount = (amount: number) => {
    return amount.toFixed(2)
  }

  const fetchData = async () => {
    dataLoading.value = true
    try {
      tableData.value = await ProductService.getList()
    } catch (error) {
      console.error('获取产品列表失败:', error)
      ElMessage.error('获取产品列表失败')
    } finally {
      dataLoading.value = false
    }
  }

  const handleCreate = () => {
    currentProduct.value = undefined
    drawerVisible.value = true
  }

  const handleEdit = (row: ProductDto) => {
    currentProduct.value = row
    drawerVisible.value = true
  }

  const handleDelete = async (row: ProductDto) => {
    try {
      await ElMessageBox.confirm(`删除后推荐人将不能再获得该产品的佣金,确定要删除吗`, '确认删除', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })

      await ProductService.delete(row.id)
      ElMessage.success('删除成功')
      fetchData()
    } catch (error) {
      if (error !== 'cancel') {
        console.error('删除产品失败:', error)
        ElMessage.error('删除产品失败')
      }
    }
  }

  const handleDrawerSuccess = () => {
    fetchData()
  }

  onMounted(() => {
    fetchData()
  })
</script>

<style scoped lang="scss"></style>
