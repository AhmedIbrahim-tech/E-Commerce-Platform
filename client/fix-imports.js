const fs = require('fs');
const path = require('path');

const replacements = [
  { from: /@\/app\/components\//g, to: '@/components/' },
  { from: /@\/app\/context\/config/g, to: '@/config/config' },
  { from: /@\/app\/context\/customizerContext/g, to: '@/store/customizerContext' },
  { from: /@\/app\/\(DashboardLayout\)\/layout\//g, to: '@/layouts/dashboard/' },
  { from: /@\/app\/\(DashboardLayout\)\/types\//g, to: '@/types/' },
  { from: /@\/app\/\(DashboardLayout\)\/utilities\//g, to: '@/utils/' },
  { from: /@\/app\/auth\/authForms\//g, to: '@/components/authForms/' },
  { from: /@\/app\/\(DashboardLayout\)/g, to: '@/app/(dashboard)' },
  { from: /@\/app\/auth\//g, to: '@/app/(auth)/' }
];

function processDirectory(dir) {
  const files = fs.readdirSync(dir);
  for (const file of files) {
    const fullPath = path.join(dir, file);
    if (fs.statSync(fullPath).isDirectory()) {
      processDirectory(fullPath);
    } else if (fullPath.endsWith('.ts') || fullPath.endsWith('.tsx')) {
      let content = fs.readFileSync(fullPath, 'utf8');
      let newContent = content;
      for (const { from, to } of replacements) {
        newContent = newContent.replace(from, to);
      }
      if (content !== newContent) {
        fs.writeFileSync(fullPath, newContent);
        console.log(`Updated ${fullPath}`);
      }
    }
  }
}

processDirectory(path.join(__dirname, 'src'));
console.log('Done!');
