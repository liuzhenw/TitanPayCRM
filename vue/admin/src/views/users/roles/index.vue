<template>
  <FullScreen>
    <div id="container">
      <el-card shadow="never" class="art-table-card">
        <ArtTable row-key="id" :data="tableData" :loading="dataLoading">
          <el-table-column key="id" prop="id" label="标识" />
          <el-table-column key="name" prop="name" label="名称" />
          <el-table-column key="isStatic" prop="isStatic" label="静态">
            <template #default="{ row }">
              <BooleanIcon :value="row.isStatic" />
            </template>
          </el-table-column>
          <el-table-column key="isPublic" prop="isPublic" label="公开">
            <template #default="{ row }">
              <BooleanIcon :value="row.isPublic" />
            </template>
          </el-table-column>
          <el-table-column key="createdAt" prop="createdAt" label="创建时间">
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
  import { RoleService, RoleDto } from '@/api/services'
  import { ref } from 'vue'

  const tableData = ref<RoleDto[]>([])
  const dataLoading = ref(false)
  const fetchData = async () => {
    dataLoading.value = true
    tableData.value = await RoleService.getList().finally(() => (dataLoading.value = false))
  }

  onMounted(() => {
    fetchData()
  })
</script>

<style scoped lang="scss"></style>
